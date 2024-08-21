using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BandProject.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [RegularExpression(@"^\w{2,}(\s+\w{2,})+$", ErrorMessage = "The full name must contain at least two words, each with at least two characters.")]
        public string FullName { get; set; }
        public double PurchasePrice { get; set; }   
        public DateTime PurchaseDate { get; set; }
    }
}
