using HPCProjectTemplate.Shared;

namespace HPCProjectTemplate.Server.Services;

public interface IUserService
{
    Task<UserDto> GetMovies(String? userName);
}
