using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Authorization;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;

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
    //IQueryable<MovieSearchResultItem>? movies { get; set; } = null;
    public List<MovieSearchResultItem> OMDBMovies { get; set; } = new List<MovieSearchResultItem>();

    public SfGrid<MovieSearchResultItem>? MoviesGrid;
    public SfToast ToastObj { get; set; } = null!;
    public string selectedPoster = String.Empty;
    public string toastContent = String.Empty;

    public MovieSearchResultItem? itemSelected = null;

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

    public async Task GetSelectedRecord(RowSelectEventArgs<MovieSearchResultItem> args)
    {
        var selectedMovie = await MoviesGrid.GetSelectedRecordsAsync();
        selectedPoster = args.Data.Poster;
        itemSelected = args.Data;
        //await AddMovie(args.Data);
    }

    public async Task ToolBarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "GridMovieAdd")
        {
            if (itemSelected is not null)
            {
                Movie newMovie = new Movie()
                {
                    imdbId = itemSelected.imdbID
                };
                var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
                if (UserAuth is not null && UserAuth.IsAuthenticated)
                {

                    var res = await Http.PostAsJsonAsync<Movie>($"api/add-movie?userName={UserAuth.Name}", newMovie);
                    if (res.IsSuccessStatusCode)
                    {
                        toastContent = $"Movie {itemSelected.Title} added to your list";
                        StateHasChanged();
                        await ToastObj.ShowAsync();
                    }
                    // add error handling to check status code
                }
            }
            else
            {
                // add error handling
            }
        }
            

    }

    private async Task GetMovies()
    {
        MovieSearchResult? movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}&page={pageNum}");

        if (movieResult is not null)
        {
            //movies = movieResult.Search.AsQueryable();
            OMDBMovies = movieResult.Search.ToList();

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

    public async Task PageClick(PagerItemClickEventArgs args)
    {
        pageNum = args.CurrentPage;
        await GetMovies();
    }


}
