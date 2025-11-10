using DogsHouse.Controllers;
using FluentAssertions;
using DogsHouse.Models;

using DogsHouse.Interfaces;
using Moq;

namespace DogsHouse.Tests.Controller;

public class DogsControllerTests
{
    private readonly Mock<IDogsService> mockDogsService;
    public DogsControllerTests()
    {
        var data = new List<Dog>
        {
            new Dog
            {
                name = "Neo",
                color = "black & white",
                tail_length = 32,
                weight = 22
            },
            new Dog {
                name = "Essy",
                color = "red",
                tail_length = 22,
                weight = 12
            },
        }.AsQueryable();


        mockDogsService = new Mock<IDogsService>();
        mockDogsService
            .Setup(s => s.GetSortedAndPaginatedDogs(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(data);
    }


    [Fact]
    public async Task DogsController_Dogs_ReturnOk()
    {
        var dogsController = new DogsController(mockDogsService.Object);
        var result = await dogsController.Dogs();
        result.Should().NotBeNull();
    }


    [Fact]
    public async Task DogsController_CreateDog_CreatesDog()
    {
        var dogsController = new DogsController(mockDogsService.Object);
        Dog dog = new Dog { name = "testDog", color = "testColor", tail_length = 1, weight = 1 };
        var result = await dogsController.CreateDog(dog);
        result.Should().NotBeNull();
    }
}


