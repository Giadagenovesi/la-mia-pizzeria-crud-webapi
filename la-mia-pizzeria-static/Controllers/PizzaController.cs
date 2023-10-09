using Azure;
using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        private ICustomLogger _myLogger;
        private PizzeriaContext _myDatabase;

        public PizzaController(PizzeriaContext db, ICustomLogger logger)
        {
            _myLogger = logger;
            _myDatabase = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            _myLogger.WriteLog("Pagina Admin gestione Pizze");
            List<Pizza> pizzas = _myDatabase.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList<Pizza>();

            return View("Index", pizzas);
        }


        //READ
        public IActionResult Dettagli(int id)
        {  
            Pizza? singlePizza = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (singlePizza == null)
            {
               return View("NotFoundPage");
            }
            else
            {
               return View("Dettagli", singlePizza);
            }
        }

        public IActionResult UserIndex()
        {
            _myLogger.WriteLog("Pagina Home Pizze");
            List<Pizza> pizzas = _myDatabase.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList<Pizza>();

            return View("UserIndex", pizzas);

        }


        //CREATE
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Aggiungi()
        {
            List<Category> categories = _myDatabase.Categories.ToList();

            List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
            List<Ingredient> dbAllIngredients = _myDatabase.Ingredients.ToList();

            foreach(Ingredient singleIngredient in dbAllIngredients)
            {
                allIngredientsSelectList.Add(new SelectListItem
                {
                    Text = singleIngredient.Name,
                    Value = singleIngredient.Id.ToString(),
                });
            }

            PizzaFormModel model =
                new PizzaFormModel { Pizza = new Pizza(), Categories = categories, Ingredients = allIngredientsSelectList };
            return View("Aggiungi", model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Aggiungi(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
                List<Ingredient> dbAllIngredients = _myDatabase.Ingredients.ToList();

                foreach (Ingredient singleIngredient in dbAllIngredients)
                {
                    allIngredientsSelectList.Add(new SelectListItem
                    {
                        Text = singleIngredient.Name,
                        Value = singleIngredient.Id.ToString(),
                    });
                }

                data.Ingredients = allIngredientsSelectList;

                return View("Aggiungi", data);
            }

            data.Pizza.Ingredients = new List<Ingredient>();

            if(data.SelectedIngredientsId != null)
            {
                foreach(string selectedId in data.SelectedIngredientsId)
                {
                    int intIngredientsId = int.Parse(selectedId);

                    Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientsId).FirstOrDefault();

                    if( ingredientInDb != null )
                    {
                        data.Pizza.Ingredients.Add(ingredientInDb);
                    }
                }
            }

            _myDatabase.Pizzas.Add(data.Pizza);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");  
        }

        //UPDATE
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Aggiorna(int id)
        {
            Pizza? pizzaToChange = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToChange == null)
            {
                return View("NotFoundPage");
            }
            else
            {
                List<Category> categories = _myDatabase.Categories.ToList();

                List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();

                foreach(Ingredient ingredient in dbIngredientList)
         {
                    allIngredientsSelectList.Add(new SelectListItem
                    {
                        Value = ingredient.Id.ToString(),
                        Text = ingredient.Name,
                        Selected = pizzaToChange.Ingredients.Any(ingredientA => ingredient.Id == ingredient.Id)
                    });
                }


                PizzaFormModel model
                    = new PizzaFormModel { Pizza = pizzaToChange, Categories = categories, Ingredients = allIngredientsSelectList };

                return View("Aggiorna", model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Aggiorna(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();

                foreach (Ingredient singleIngredient in dbIngredientList)
                {
                    allIngredientsSelectList.Add(new SelectListItem
                    {
                        Value = singleIngredient.Id.ToString(),
                        Text = singleIngredient.Name
                    });
                }

                data.Ingredients = allIngredientsSelectList;

                return View("Aggiorna", data); ;
            }

            // Variante più complessa e astratta, ma alla lunga più immediata
            Pizza? pizzaToUpdate = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToUpdate != null)
            {
                pizzaToUpdate.Ingredients.Clear();

                pizzaToUpdate.Image = data.Pizza.Image;
                pizzaToUpdate.Taste = data.Pizza.Taste;
                pizzaToUpdate.Price = data.Pizza.Price;
                pizzaToUpdate.Description = data.Pizza.Description;
                pizzaToUpdate.CategoryId = data.Pizza.CategoryId;
                


                if (data.SelectedIngredientsId != null)
                {
                    foreach (string ingredientSelectedId in data.SelectedIngredientsId)
                    {
                        int intIngredientSelectedId = int.Parse(ingredientSelectedId);

                        Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                        if (ingredientInDb != null)
                        {
                            pizzaToUpdate.Ingredients.Add(ingredientInDb);
                        }
                    }
                }

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View("NotFoundPage");
            }

        }
        //DELETE
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancella(int id)
        {
            Pizza? pizzaToDelete = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _myDatabase.Pizzas.Remove(pizzaToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View("NotFoundPage");
            }
        }
    }
}
