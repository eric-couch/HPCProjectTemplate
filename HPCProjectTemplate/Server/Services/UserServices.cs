using HPCProjectTemplate.Server.Data;
using HPCProjectTemplate.Server.Controllers;
using HPCProjectTemplate.Server.Models;
using HPCProjectTemplate.Shared;
using HPCProjectTemplate.Shared.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

    public async Task<Response> AddMovie(Movie movie, String? userName)
    {
        try
        {
            var user = await (from u in _context.Users
                              where u.UserName == userName
                              select u).FirstOrDefaultAsync();
            if (user is null)
            {
                return new Response(false, "Could not find user.");
            }
            user.FavoriteMovies.Add(movie);
            _context.SaveChanges();
            return new Response(true, $"Added movie: {movie.imdbId} ");
        } catch (Exception ex)
        {
            return new Response(false, ex.Message);
        }
        

    }

    public async Task<Response> RemoveMovie(Movie movie, String? userName)
    {
        var movieToRemove = _context.Users.Include(u => u.FavoriteMovies)
                            .FirstOrDefault(u =>  u.UserName == userName)
                            .FavoriteMovies.FirstOrDefault(m => m.imdbId == movie.imdbId);
        _context.Movies.Remove(movieToRemove);
        _context.SaveChanges();
        return new Response(true, $"Removed movie: {movie.imdbId} ");
    }
}
