using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly string _secretKey;
        public UserController(DataContext context, IConfiguration configuration) 
        {
            _context = context;
            _secretKey = configuration["AppSettings:SecretKey"];
        }
        [HttpPost("Login")]
        public IActionResult ValidateUser(LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.UserName == model.UserName && p.Password == model.Password);
            if (user == null)
            {
                return Ok(new ResponseApi
                {
                    Success = false,
                    Message = "Login Fail",
                });
            }
            var token = GenerateToken(user);
            return Ok(new ResponseApi
            {
                Success = true,
                Message = "Login Successfully",
                Data = token
            });
        }
        private TokenModel GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var claim = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString()),
                };
            if (user.Role == "Admin")
                claim.Add(new Claim("AdminView", "true"));
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDesc);
            var accessToken =  jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Save in DB
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),
                UserId = user.Id,
            };
            _context.RefreshTokens.Add(refreshTokenEntity);
            _context.SaveChanges(); 
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var tokenValidateParam = new TokenValidationParameters()
            {
                //auto provide token
                ValidateIssuer = false,
                ValidateAudience = false,

                //Add signature to Token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // Không kiểm tra hạn của token
            };
            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validateToken);
                if(validateToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if(!result)
                    {
                        return BadRequest(new ResponseApi {Success = false, Message = "Invalid Token" });
                    }
                }
                // Check AccessToken expired?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = DateTimeOffset.FromUnixTimeSeconds(utcExpireDate).DateTime; 
                 if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new ResponseApi { Success = false, Message = "Not yet Expired" });
                }

                //Check refresh token exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ResponseApi { Success = false, Message = "Refresh token does not exist" });
                }
                //Check refresh token is used or revoked 
                if(storedToken.IsUsed || storedToken.IsRevoked)
                    return Ok(new ResponseApi { Success = false, Message = "Refresh token has been Used or Revoked." });

                //Check Access token == jwtId in Refresh token
                var jwtId = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.JwtId != jwtId)
                    return Ok(new ResponseApi { Success = false, Message = "Token not match." });

                //Update refresh token in DB
                storedToken.IsUsed = true;
                storedToken.IsRevoked = true;
                _context.Update(storedToken);
                _context.SaveChanges();

                //Create new Token
                var user = _context.Users.SingleOrDefault( x => x.Id == storedToken.UserId );
                if(user == null)
                    return BadRequest();
                var token = GenerateToken(user);
                return Ok(new ResponseApi
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi { Success=false, Message = ex.Message });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTimeInterval = dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
