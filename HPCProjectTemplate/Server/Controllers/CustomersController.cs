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
}
