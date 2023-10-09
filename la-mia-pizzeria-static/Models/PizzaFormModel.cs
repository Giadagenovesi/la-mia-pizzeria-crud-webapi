using Microsoft.AspNetCore.Mvc.Rendering;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set; }

        //Gestisco le proprietà  per gestire la select multipla nelle view
        public List<SelectListItem>? Ingredients { get; set; }
        public List<string>? SelectedIngredientsId { get; set; }

        
    }
}
