using aspnetcore_with_reactspa.Models;
using aspnetcore_with_reactspa.Data;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_with_reactspa.Services;

public class PizzaService
{
    private readonly PizzaContext _context;

    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<Topping> ToppingGetAll()
    {
        Console.WriteLine(_context.Toppings.AsNoTracking().ToList().Count());
        return _context.Toppings
            .AsNoTracking()
            .ToList();

    }

    public IEnumerable<Sauce> SauceGetAll()
    {
        return _context.Sauces
            .AsNoTracking()
            .ToList();

    }

    public IEnumerable<Pizza> GetAll()
    {
        return _context.Pizzas
            .AsNoTracking()
            .ToList();

    }


    public Pizza? GetById(int id)
    {
        return _context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);

    }

    public Pizza? Create(Pizza newPizza)
    {

        var pizza = new Pizza
        {
            Id = newPizza.Id,
            Name = newPizza.Name
        };

        _context.Pizzas.Add(pizza);
        _context.SaveChanges();


        UpdateSauce(pizza.Id, newPizza.Sauce.Id);

        foreach (var item in newPizza.Toppings)
        {
            AddTopping(pizza.Id, item.Id);
        }


        return newPizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var toppingToAdd = _context.Toppings.Find(ToppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new NullReferenceException("Pizza or topping does not exist");
        }

        if (pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        pizzaToUpdate.Toppings.Add(toppingToAdd);

        _context.Pizzas.Update(pizzaToUpdate);
        _context.SaveChanges();

    }

    public void UpdatePizza(int PizzaId, Pizza newPizza)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        pizzaToUpdate = _context.Pizzas
        .Include(p => p.Toppings)
        .SingleOrDefault(p => p.Id == PizzaId);


        if (pizzaToUpdate is null)
        {
            throw new NullReferenceException("Pizza or sauce does not exist");
        }

        pizzaToUpdate.Name = newPizza.Name;
        pizzaToUpdate.Sauce = newPizza.Sauce;


        //_context.Toppings.Find
        //intenté eliminar los anteriores ingredientes que tuviese la pizza a eliminar
        //y después agregar de nuevo uno por uno, pero no jaló
        //Hace falta actualizar los ingredientes
        var toppingsPizza = pizzaToUpdate.Toppings.ToList();

        foreach (var item in toppingsPizza)
        {
            pizzaToUpdate.Toppings.Remove(item);
            Console.WriteLine(item);


        }
        _context.SaveChanges();

        foreach (var item in newPizza.Toppings.ToList())
        {
            AddTopping(newPizza.Id, item.Id);

        }

        _context.SaveChanges();

    }

    public void UpdateSauce(int PizzaId, int SauceId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var sauceToUpdate = _context.Sauces.Find(SauceId);

        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new NullReferenceException("Pizza or sauce         does not exist");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;

        _context.SaveChanges();

    }

    public void DeleteById(int id)
    {
        var pizzaDel = _context.Pizzas.Find(id);
        if (pizzaDel is null)
        {
            throw new NullReferenceException("Pizza does not exist");
        }
        _context.Pizzas.Remove(pizzaDel);
        _context.SaveChanges();
    }
}