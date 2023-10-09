using la_mia_pizzeria_static.Validation;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        public int Id { get; set; }

        [Url(ErrorMessage = "Devi inserire un link valido ad un'immagine")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Una pizza deve per forza avere un gusto")]
        [MaxLength(100, ErrorMessage = "La massima lunghezza del campo è di 100 caratteri")]
        public string Taste { get; set; }

        [Required(ErrorMessage = "Non può esistere una pizza senza ingredienti")]
        [MinimumWord]
        [MaxLength(300, ErrorMessage = "La massima lunghezza del campo è di 300 caratteri")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Non siamo un ente di beneficenza, inserisci un prezzo per questa pizza!")]
        public float Price { get; set; }


        // Relazione 1:* con la categoria
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        // Relazione *:* con gli ingredienti
        public List<Ingredient>? Ingredients { get; set; }

        public Pizza() { }

        public Pizza(string image, string taste, string description, float price)
        {
            Image = image;
            Taste = taste;
            Description = description;
            Price = price;
        }
    }
}
