using HPCProjectTemplate.Shared;
using HPCProjectTemplate.Shared.Wrappers;
using System.Net.Http.Json;
using System.Net;


namespace HPCProjectTemplate.Client.HttpRepository;

public class UserMoviesHttpRepository : IUserMoviesHttpRepository
{
    public readonly HttpClient _httpClient;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    public UserMoviesHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // wrapper pattern
    public async Task<DataResponse<List<OMDBMovieResponse>>> GetMovies(string userName) {
        try
        {
            var MovieDetails = new List<OMDBMovieResponse>();
            UserDto? user = await _httpClient.GetFromJsonAsync<UserDto>($"api/get-movies?userName={userName}");
            if (user?.FavoriteMovies?.Any() ?? false)
            {
                foreach (Movie movie in user.FavoriteMovies)
                {
                    var movieDetails = await _httpClient.GetFromJsonAsync<OMDBMovieResponse>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    MovieDetails.Add(movieDetails);
                }
            }
            return new DataResponse<List<OMDBMovieResponse>>()
            {
                Data = MovieDetails,
                Message = "Success",
                Success = true
            };
        } catch (Exception e)
        {
            Console.WriteLine(e);
            return new DataResponse<List<OMDBMovieResponse>>()
            {
                Data = new List<OMDBMovieResponse>(),
                Message = e.Message,
                Success = false,
                Errors = new Dictionary<string, string[]> { { e.Message, new string[] { e.Message } } }
            };
        }
    }

}
