using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Authorization;

namespace HPCProjectTemplate.Client.Pages;

public partial class Search
{
    [Inject]
    HttpClient Http { get; set; } = null!;
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    public string SearchTitle { get; set; }
    public MovieSearchResult? movieSearchResult { get; set; }
    //GridItemsProvider<MovieSearchResultItem>? gridItemsProvider;
    IQueryable<MovieSearchResultItem>? movies { get; set; } = null;

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    private int pageNum = 1;
    private int totalPages = 2;
    private int totalResults = 0;



    private async Task SearchOMDB()
    {
        pageNum = 1;
        await GetMovies();
        StateHasChanged();
    }

    private async Task NextPage()
    {
        if (pageNum < totalPages)
        {
            pageNum++;

        }
        await GetMovies();
        StateHasChanged();
    }

    private async Task PreviousPage()
    {
        if (pageNum > 1)
        {
            pageNum--;

        }
        await GetMovies();
        StateHasChanged();
    }

    private async Task GetMovies()
    {
        MovieSearchResult? movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}&page={pageNum}");

        if (movieResult is not null)
        {
            movies = movieResult.Search.AsQueryable();
            if (Double.TryParse(movieResult.totalResults, out double total))
            {
                totalResults = (int)total;
                totalPages = (int)Math.Ceiling(totalResults / 10.0);
            }
            else
            {
                totalPages = 0;
            }
        }
    }

    private async Task AddMovie(MovieSearchResultItem movie)
    {
        Movie newMovie = new Movie()
        {
            imdbId = movie.imdbID
        };

        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {

            await Http.PostAsJsonAsync<Movie>($"api/add-movie?userName={UserAuth.Name}", newMovie);
        }
    }

}
