using Moq;
using RichardSzalay.MockHttp;
using HPCProjectTemplate.Client.HttpRepository;
using HPCProjectTemplate.Shared;
using HPCProjectTemplate.Shared.Wrappers;

namespace HPCProjectTemplate.Client.Test;

public class Tests
{
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetMovies_ShouldReturnDataResponseMovieList_WhenUserExists()
    {
        var mockHttp = new MockHttpMessageHandler();
        string testUserResponse = """
            {
                "id": "6db935b1-05cd-4acc-a5f9-1d1ef22db836",
                "userName": "ecouch@example.net",
                "firstName": "Eric",
                "lastName": "Couch",
                "favoriteMovies": [
                    {   "id": 1,
                        "imdbId": "tt1216475"
                    }
                ]
            }
            """;
        string testOMDBCars2Response = """
            {
                "Title": "Cars 2",
                "Year": "2011",
                "Rated": "G",
                "Released": "24 Jun 2011",
                "Runtime": "106 min",
                "Genre": "Animation, Adventure, Comedy, Family, Sport",
                "Director": "John Lasseter, Brad Lewis(co-director)",
                "imdbId": "tt1216475",
                "imdbRating": "6.2"
            }
            """;
        // https://localhost:7108/api/get-movies?userName=ecouch@example.net
        string? userName = "ecouch@example.net";
        mockHttp.When($"https://localhost:7108/api/get-movies?userName={userName}")
                .Respond("application/json", testUserResponse);
        //              https://www.omdbapi.com/?apikey=86c39163&i=tt1216475
        mockHttp.When($"https://www.omdbapi.com/?apikey=86c39163&i=tt1216475")
                .Respond("application/json", testOMDBCars2Response);
        
        var client = new HttpClient(mockHttp);
        client.BaseAddress = new Uri("https://localhost:7108");
        var userMoviesHttpRepository = new UserMoviesHttpRepository(client);

        var dataResponse = await userMoviesHttpRepository.GetMovies(userName);
        var response = dataResponse.Data;

        Assert.AreEqual(response[0].Title, "Cars 2");
        Assert.AreEqual(response[0].Year, "2011");
        Assert.AreEqual(response[0].Rated, "G");
        Assert.AreEqual(response[0].Released, "24 Jun 2011");
        Assert.AreEqual(response[0].Runtime, "106 min");
        Assert.AreEqual(response[0].Genre, "Animation, Adventure, Comedy, Family, Sport");
        Assert.AreEqual(response[0].Director, "John Lasseter, Brad Lewis(co-director)");
        
    }
}