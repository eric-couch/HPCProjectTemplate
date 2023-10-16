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
        // Arrange
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
                    },
                    {   "id": 2,
                        "imdbId": "tt0372784"
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

        string testOMDBBatmanBeginsResponse = """
            {
                "Title": "Batman Begins",
                "Year": "2005",
                "Rated": "PG-13",
                "Released": "15 Jun 2005",
                "Runtime": "140 min",
                "Genre": "Action, Adventure",
                "Director": "Christopher Nolan",
                "imdbId": "tt0372784",
                "imdbRating": "8.2"
            }
            """;

        string? userName = "ecouch@example.net";
        mockHttp.When($"https://localhost:7108/api/get-movies?userName={userName}")
                .Respond("application/json", testUserResponse);

        mockHttp.When($"https://www.omdbapi.com/?apikey=86c39163&i=tt1216475")
                .Respond("application/json", testOMDBCars2Response);

        mockHttp.When($"https://www.omdbapi.com/?apikey=86c39163&i=tt0372784")
                .Respond("application/json", testOMDBBatmanBeginsResponse);

        var client = new HttpClient(mockHttp);
        client.BaseAddress = new Uri("https://localhost:7108");
        var userMoviesHttpRepository = new UserMoviesHttpRepository(client);
        
        // Act
        var dataResponse = await userMoviesHttpRepository.GetMovies(userName);

        // Assert
        if (dataResponse.Success)
        {
            var response = dataResponse.Data;
            // Assert Cars 2 values
            Assert.AreEqual(response[0].Title, "Cars 2");
            Assert.AreEqual(response[0].Year, "2011");
            Assert.AreEqual(response[0].Rated, "G");
            Assert.AreEqual(response[0].Released, "24 Jun 2011");
            Assert.AreEqual(response[0].Runtime, "106 min");
            Assert.AreEqual(response[0].Genre, "Animation, Adventure, Comedy, Family, Sport");
            Assert.AreEqual(response[0].Director, "John Lasseter, Brad Lewis(co-director)");

            // Assert Batman Begins values
            Assert.AreEqual(response[1].Title, "Batman Begins");
            Assert.AreEqual(response[1].Year, "2005");
            Assert.AreEqual(response[1].Rated, "PG-13");
            Assert.AreEqual(response[1].Released, "15 Jun 2005");
            Assert.AreEqual(response[1].Runtime, "140 min");
            Assert.AreEqual(response[1].Genre, "Action, Adventure");
            Assert.AreEqual(response[1].Director, "Christopher Nolan");
        } else
        {
            Assert.Fail();
        }
        

         
        
        
    }
}