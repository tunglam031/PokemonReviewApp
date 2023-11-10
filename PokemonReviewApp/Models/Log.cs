using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public DateTime CreateOn { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public string Stacktrace { get; set; }
    }
}
