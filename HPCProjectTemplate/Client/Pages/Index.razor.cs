using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using HPCProjectTemplate.Shared;
using System.Net.Http.Json;

namespace HPCProjectTemplate.Client.Pages;

public partial class Index
{
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject]
    public HttpClient Http { get; set; } = null!;
    public UserDto? User { get; set; } = null;
    public List<OMDBMovieResponse> movies { get; set; } = new List<OMDBMovieResponse>();

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";

    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            User = await Http.GetFromJsonAsync<UserDto>($"api/get-movies?userName={UserAuth.Name}");
            if (User is not null)
            {
                foreach (var movie in User.FavoriteMovies)
                {
                    OMDBMovieResponse omdbMovie = await Http.GetFromJsonAsync<OMDBMovieResponse>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    movies.Add(omdbMovie);
                }
                
            }
        }
        
    }
}
