﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using HPCProjectTemplate.Shared;
using System.Net.Http.Json;
using HPCProjectTemplate.Client.HttpRepository;
using HPCProjectTemplate.Shared.Wrappers;
using System.Diagnostics;
using Microsoft.JSInterop;
using Blazored.LocalStorage;

namespace HPCProjectTemplate.Client.Pages;

public partial class Index
{
    [Inject]
    public ILocalStorageService localStorage { get; set; }
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject]
    public HttpClient Http { get; set; } = null!;
    [Inject]
    public IUserMoviesHttpRepository UserMoviesHttpRepository { get; set; } = null!;
    [Inject]
    public IJSRuntime JS { get; set; } = null!;
    public UserDto? User { get; set; } = null;
    public List<OMDBMovieResponse> movies { get; set; } = new List<OMDBMovieResponse>();
    public bool isVisible = true;

    //private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    //private readonly string OMDBAPIKey = "86c39163";

    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            DataResponse<List<OMDBMovieResponse>> dataResponse = await UserMoviesHttpRepository.GetMovies(UserAuth.Name);
            if (dataResponse.Success)
            {
                movies = dataResponse.Data;
                foreach (var movie in movies)
                {
                    await localStorage.SetItemAsync(movie.imdbID, movie.Title);
                }
                // log this
                // show the user a message "success" - this is a toast notification
            } else
            {
                Console.WriteLine(dataResponse.Message);
                Debug.WriteLine(dataResponse.Message);
                // show the user a message "failure"
            }
        }
        isVisible = false;
    }

    private async Task RemoveFavoriteMovie(OMDBMovieResponse movie)
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            bool confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to remove that movie?");
            if (confirmed)
            {
                await UserMoviesHttpRepository.RemoveMovie(movie.imdbID, UserAuth.Name);
                //await JS.InvokeVoidAsync("alert", "Movie was removed");
                await JS.InvokeVoidAsync("userFeedback", $"Removed {movie.Title} from user favorites.");
                movies.Remove(movie);
                StateHasChanged();
            }
            
            await Task.CompletedTask;
        }
    }
}
