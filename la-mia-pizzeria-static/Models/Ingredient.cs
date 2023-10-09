using System.Text.Json.Serialization;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }

        // Relazione *:* con le Pizze
        public List<Pizza>? Pizzas { get; set; }

        public Ingredient()
        {

        }
    }
}
