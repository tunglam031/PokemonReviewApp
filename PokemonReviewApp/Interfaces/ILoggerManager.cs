namespace PokemonReviewApp.Interfaces
{
    public interface ILoggerManager
    {
        void ErrorLog(Exception ex);
        void InfoLog(string message);


    }
}
