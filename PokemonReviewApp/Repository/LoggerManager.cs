using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class LoggerManager : ILoggerManager
    {
        private readonly DataContext _context;
        public LoggerManager(DataContext context) 
        {
            _context = context;
        }
        public void ErrorLog(Exception ex)
        {
            var newLog = new Log
            {
                CreateOn = DateTime.Now,
                Message = ex.Message,
                Level = "E",
                Stacktrace = ex.StackTrace,
                Exception = ex.ToString()
            };
            _context.Logs.Add(newLog);
        }

        public void InfoLog(string message)
        {
            var newLog = new Log
            {
                CreateOn = DateTime.Now,
                Message = message,
                Level = "I"
            };
        }
    }
}
