using HPCProjectTemplate.Shared;
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

    public async Task<List<OMDBMovieResponse>> GetMovies(string userName) {
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
            return MovieDetails;
        } catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<OMDBMovieResponse>();
        }
    }

}
