using HPCProjectTemplate.Shared;
using HPCProjectTemplate.Shared.Wrappers;

namespace HPCProjectTemplate.Server.Services;

public interface IUserService
{
    Task<UserDto> GetMovies(String? userName);
    Task<Response> AddMovie(Movie movie, String? userName);
    Task<Response> RemoveMovie(Movie movie, String? userName);

}
