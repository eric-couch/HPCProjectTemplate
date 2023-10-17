using HPCProjectTemplate.Server.Data;
using HPCProjectTemplate.Server.Models;
using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;
using HPCProjectTemplate.Server.Services;

namespace HPCProjectTemplate.Server.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/get-movies?userName=userName
    [HttpGet("api/get-movies")]
    public async Task<ActionResult<UserDto>> GetMovies([FromQuery(Name = "userName")] string userName)
    {
        var movies = await _userService.GetMovies(userName);
        if (movies is null)
        {
            return NotFound();
        }
        return Ok(movies);
    }

    //[HttpPost("api/add-movie")]
    //public async Task<ActionResult> AddMovie([FromBody] Movie movie, [FromQuery(Name = "userName")] string userName)
    //{
    //    var user = await (from u in _context.Users
    //                      where u.UserName == userName
    //                      select u).FirstOrDefaultAsync();
    //    user.FavoriteMovies.Add(movie);
    //    _context.SaveChanges();

    //    return Ok();
    //}
}
