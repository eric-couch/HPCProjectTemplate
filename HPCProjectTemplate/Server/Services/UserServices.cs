using HPCProjectTemplate.Server.Data;
using HPCProjectTemplate.Server.Controllers;
using HPCProjectTemplate.Server.Models;
using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HPCProjectTemplate.Server.Services;

public class UserServices : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserServices> _logger;

    public UserServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<UserServices> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UserDto> GetMovies(String? userName)
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
        _logger.LogInformation("User {UserName} retrieving {Count} favorite movies.  Logged on {PlaceHolderName:MMMM dd, yyyy}", userMovies.UserName, userMovies.FavoriteMovies.Count, DateTimeOffset.UtcNow);

        return userMovies;
    }

}
