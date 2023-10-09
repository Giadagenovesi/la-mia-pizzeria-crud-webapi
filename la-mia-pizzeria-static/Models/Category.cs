using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome della categoria è obbligatorio!")]
        [StringLength(50, ErrorMessage = "Il Nome della categoria non può superare i 50 caratteri")]
        public string Name { get; set; }

        // Relazione 1:* con le pizze
        public List<Pizza>? Pizzas { get; set; }

        public Category()
        {

        }
    }
}
