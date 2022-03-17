using aspnetcore_with_reactspa.Services;
using aspnetcore_with_reactspa.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_with_reactspa.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    PizzaService _service;
    
    public PizzaController(PizzaService service)
    {
        _service = service;
    }

    //localhost/pizza/sauce
    [HttpGet("sauce")]
    public IEnumerable<Sauce> SauceGetAll()
    {
        return _service.SauceGetAll();
    }

    //localhost/pizza/topping
    [HttpGet]
    [Route("topping")]
    public IEnumerable<Topping> ToppingGetAll()
    {
        return _service.ToppingGetAll();
    }


     [HttpGet]
    public IEnumerable<Pizza> GetAllPizzaToppings()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Pizza> GetById(int id)
    {
        var pizza = _service.GetById(id);

        if(pizza is not null)
        {
            return pizza;
        }
        else
        {
            return NotFound();
        }
    }


    [HttpPost]
    public IActionResult Create(Pizza newPizza)
    {
        var pizza = _service.Create(newPizza);
        return CreatedAtAction(nameof(GetById), new { id = pizza!.Id }, pizza);
    }

    [HttpPut("{id}/UpdatePizza")]
    public IActionResult UpdatePizza(int id, Pizza newPizza)
    {
        var pizzaToUpdate = _service.GetById(id);

        if(pizzaToUpdate is not null)
        {
            Console.WriteLine("Entró aquí");
            //Console.WriteLine(newPizza.ToString());
            _service.UpdatePizza(id, newPizza);
            return NoContent();    
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/addtopping")]
    public IActionResult AddTopping(int id, int toppingId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if(pizzaToUpdate is not null)
        {
            _service.AddTopping(id, toppingId);
            return NoContent();    
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/updatesauce")]
    public IActionResult UpdateSauce(int id, int sauceId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if(pizzaToUpdate is not null)
        {
            _service.UpdateSauce(id, sauceId);
            return NoContent();    
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _service.GetById(id);

        if(pizza is not null)
        {
            _service.DeleteById(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}