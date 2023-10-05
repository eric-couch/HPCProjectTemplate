using HPCProjectTemplate.Server.Data;
using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;

namespace HPCProjectTemplate.Server.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
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
        return Ok(userMovies);
    }
}
