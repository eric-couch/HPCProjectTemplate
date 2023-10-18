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

    [HttpPost("api/add-movie")]
    public async Task<ActionResult> AddMovie([FromBody] Movie movie, [FromQuery(Name = "userName")] string userName)
    {
        var response = await _userService.AddMovie(movie, userName);

        if (response is not null && response.Success) 
        {
            return Ok(); 
        }
        return NotFound();
    }

    [HttpPost("api/remove-movie")]
    public async Task<ActionResult<bool>> RemoveMovie([FromBody] Movie movie, [FromQuery(Name = "userName")] string userName)
    {
        try
        {
            var response = await _userService.RemoveMovie(movie, userName);
            if (response is not null && response.Success)
            {
                return Ok();
            }
            return NotFound();
        } catch (Exception ex) {
            return NotFound();
        }
    }
}
