using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {

        //Faccio una chiamata API che mi restituisce la lista di tutte le mie pizze
        [HttpGet]
        public IActionResult GetPizzas()
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                List<Pizza> pizzas = db.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();

                return Ok(pizzas);
            }
        }

        //Faccio una chiamata API che mi restituisce la lista di tutte le mie pizze che contengono quella determinata stringa nel nome
        [HttpGet]
        public IActionResult GetPizzasByName(string? research)
        {
            if (research == null)
            {
                return BadRequest();
            }

            using (PizzeriaContext db = new PizzeriaContext())
            {

                List<Pizza> pizzasResult = db.Pizzas.Where(Pizza => Pizza.Taste.ToLower().Contains(research.ToLower())).ToList();

                return Ok(pizzasResult);
            }
        }

        //Faccio una chiamata API che mi restituisce la lista di tutte le mie pizze che contengono quella determinata stringa nel nome
        [HttpGet]
        public IActionResult GetPizzasById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            using (PizzeriaContext db = new PizzeriaContext())
            {

                Pizza pizza = db.Pizzas.Where(Pizza => Pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

                return Ok(pizza);
            }
        }
    }

}
