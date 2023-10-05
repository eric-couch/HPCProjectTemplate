using Microsoft.AspNetCore.Identity;
using HPCProjectTemplate.Shared;

namespace HPCProjectTemplate.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public List<Movie> FavoriteMovies { get; set; } = new();
    }
}