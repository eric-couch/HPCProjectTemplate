using HPCProjectTemplate.Server.Data;
using HPCProjectTemplate.Server.Models;
using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;

namespace HPCProjectTemplate.Server.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: api/get-movies?userName=userName
    [HttpGet("api/get-movies")]
    public async Task<ActionResult<UserDto>> GetMovies([FromQuery(Name = "userName")] string userName)
    {
        UserDto? userMovies = await _context.Users
                                    .Include(m => m.FavoriteMovies)
                                    .Select(u => new UserDto
                                    {
                                        Id = u.Id,
                                        UserName = u.UserName,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        FavoriteMovies = u.FavoriteMovies
                                    }).FirstOrDefaultAsync(u => u.UserName == userName);
        /* UserDto? userMovies = await (from u in _context.Users
                                        join m in _context.Movies 
                                        on u.Id equals m.Id
                                        where u.UserName == userName
                                        select new UserDto
                                        {
                                            Id = u.Id,
                                            UserName = u.UserName,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            FavoriteMovies = u.FavoriteMovies
                                        }).FirstOrDefaultAsync(); */
        if (userMovies is null)
        {
            return NotFound();
        }
        return Ok(userMovies);
    }

    [HttpPost("api/add-movie")]
    public async Task<ActionResult> AddMovie([FromBody] Movie movie, [FromQuery(Name = "userName")] string userName)
    {
        var user = await (from u in _context.Users
                          where u.UserName == userName
                          select u).FirstOrDefaultAsync();
        user.FavoriteMovies.Add(movie);
        _context.SaveChanges();

        return Ok();
    }
}
