using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace HPCProjectTemplate.Client.Pages;

public partial class Search
{
    [Inject]
    HttpClient Http { get; set; } = null!;
    public string SearchTitle { get; set; }
    public MovieSearchResult? movieSearchResult { get; set; }

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";

    private async Task SearchOMDB()
    {
        movieSearchResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}");

    }
}
