using HPCProjectTemplate.Shared;
using HPCProjectTemplate.Shared.Wrappers;

namespace HPCProjectTemplate.Client.HttpRepository;

public interface IUserMoviesHttpRepository
{
    Task<DataResponse<List<OMDBMovieResponse>>> GetMovies(string userName);
    Task<bool> RemoveMovie(string imdbId, string userName);
}
