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
        private PizzeriaContext _myDatabase;

        public PizzasController(PizzeriaContext db)
        {
            _myDatabase = db;
        }

        //Faccio una chiamata API che mi restituisce la lista di tutte le mie pizze
        [HttpGet]
        public IActionResult GetPizzas()
        {

            List<Pizza> pizzas = _myDatabase.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();

            return Ok(pizzas);

        }

        //Faccio una chiamata API che mi restituisce la lista di tutte le mie pizze che contengono quella determinata stringa nel nome
        [HttpGet]
        public IActionResult GetPizzasByName(string? research)
        {
            if (research == null)
            {
                return BadRequest();
            }



            List<Pizza> pizzasResult = _myDatabase.Pizzas.Where(Pizza => Pizza.Taste.ToLower().Contains(research.ToLower())).ToList();

            return Ok(pizzasResult);

        }

        //Faccio una chiamata API che mi restituisce la lista di tutte le mie pizze che contengono quella determinata stringa nel nome
        [HttpGet]
        public IActionResult GetPizzasById(int? id)
        {



            Pizza pizza = _myDatabase.Pizzas.Where(Pizza => Pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();


            if (id != null)
            {
                return Ok(pizza);
            }
            else
            {
                return NotFound();
            }



        }

        //Creo una nuova pizza tramite chiamata API
        [HttpPost]

        public IActionResult CreateNewPizza([FromBody] Pizza newPizza)
        {
            try
            {
                _myDatabase.Pizzas.Add(newPizza);
                _myDatabase.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Modifico una pizza in base al suo id
        [HttpPut("{id}")]

        public IActionResult EditPizza(int id, [FromBody] Pizza updatedPizza)
        {
            Pizza? pizzaToUpdate = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToUpdate == null)
            {
                return NotFound();
            }

            pizzaToUpdate.Image = updatedPizza.Image;
            pizzaToUpdate.Taste = updatedPizza.Taste;
            pizzaToUpdate.Price = updatedPizza.Price;
            pizzaToUpdate.Description = updatedPizza.Description;


            _myDatabase.SaveChanges();


            return Ok();

        }

        // Elimino una pizza in base al suo id

        [HttpDelete("{id}")]

        public IActionResult DeletePizza(int id)
        {
            Pizza? pizzaToDelete = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _myDatabase.Pizzas.Remove(pizzaToDelete);
                _myDatabase.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }

}
