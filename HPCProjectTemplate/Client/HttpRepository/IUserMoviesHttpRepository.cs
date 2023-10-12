using HPCProjectTemplate.Shared;

namespace HPCProjectTemplate.Client.HttpRepository;

public interface IUserMoviesHttpRepository
{
     Task<List<OMDBMovieResponse>> GetMovies(string userName);
}
