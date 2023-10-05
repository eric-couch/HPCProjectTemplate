using HPCProjectTemplate.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HPCProjectTemplate.Shared;
using HPCProjectTemplate.Server.Models;

namespace HPCProjectTemplate.Server.Controllers;


public class CustomersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ApplicationDbContext dbContext, ILogger<CustomersController> logger)
    {
        _context = dbContext;
        _logger = logger;

    }
    [HttpGet]
    [Route("api/customers")]
    public async Task<List<Customer>> Index()
    {
        try
        {
            _logger.LogInformation("CustomersController.Index() called");
            return await _context.Customers.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error {ex.Message}");
            return new List<Customer>();
        }
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
