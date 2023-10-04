using HPCProjectTemplate.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPCProjectTemplate.Server.Controllers;
using HPCProjectTemplate.Server.Models;

public class CustomersController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    [HttpGet]
    [Route("api/customers")]
    public async Task<List<Customer>> Index()
    {
        return await _context.Customers.ToListAsync();
    }

    [HttpGet]
    [Route("api/customers/{id}")]
    public async Task<Customer> GetCustomer(int id)
    {
        Customer customer = await _context.Customers.FindAsync(id);
        if (customer is not null)
        {
            return customer;
        }
        else
        {
            return new Customer();
        }
    }

    [HttpGet]
    [Route("api/customers/delete/{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        Customer customer = await _context.Customers.FindAsync(id);
        if (customer is not null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
