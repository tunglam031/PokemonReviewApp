using Microsoft.AspNetCore.Mvc;
using NLog;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLogController : Controller
    {
        private readonly ILogger<TestLogController> _logger;
        public TestLogController(ILogger<TestLogController> logger) 
        {
            _logger = logger;
        }

        [HttpGet]
        public int Test (int x, int y)
        {
            try
            {
                return x / y;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
