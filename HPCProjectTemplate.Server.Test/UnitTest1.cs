using HPCProjectTemplate.Server.Controllers;
using HPCProjectTemplate.Server.Services;
using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Moq;

namespace HPCProjectTemplate.Server.Test;

public class Tests
{
    private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetMovies_ShouldReturnDto()
    {
        // Arrange
        UserDto user = new UserDto()
        {
            Id = "6db935b1-05cd-4acc-a5f9-1d1ef22db836",
            UserName = "ecouch@example.net",
            FirstName = "Eric",
            LastName = "Couch",
            FavoriteMovies = new List<Movie>() {
                new Movie() {
                    Id = 1,
                    imdbId = "tt1216475"
                },
                new Movie() {
                    Id = 2,
                    imdbId = "tt0372784"
                },
                new Movie() {
                    Id = 3,
                    imdbId = "tt0066999"
                }
            }
        };
        string? userName = "ecouch@example.net";
        _userServiceMock.Setup(x => x.GetMovies(userName)).ReturnsAsync(user);

        UserController userController = new UserController(_userServiceMock.Object);

        // Act
        var response = await userController.GetMovies(userName);
        var result = (OkObjectResult)response.Result!;
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        var userDto = (UserDto)result.Value!;

        // Assert
        Assert.That(userDto, Is.TypeOf<UserDto>());
        Assert.That(userDto.UserName, Is.EqualTo("ecouch@example.net"));
        Assert.That(userDto.FirstName, Is.EqualTo("Eric"));
        Assert.That(userDto.LastName, Is.EqualTo("Couch"));
        Assert.That(userDto.FavoriteMovies[0].imdbId, Is.EqualTo("tt1216475"));
    }
}