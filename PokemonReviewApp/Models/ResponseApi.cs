namespace PokemonReviewApp.Models
{
    public class ResponseApi
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TokenModel Data { get; set; }
    }
}
