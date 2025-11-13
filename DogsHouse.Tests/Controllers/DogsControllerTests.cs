

using BLL.Interfaces;
using BLL.Services;
using DAL.Context;
using DAL.Repository;
using DAL.Repository.Interfaces;
using DAL.Repository.UnitOfWork;
using DogsHouse.Controllers;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DogsHouse.Tests.Controller;

public class DogsControllerTests
{
    private List<Dog> data = new List<Dog>
        {
            new Dog
            {
                name = "Neo",
                color = "red & amber",
                tail_length = 22,
                weight = 32
            },
            new Dog
            {
                name = "Jessy",
                color = "black & white",
                tail_length = 7,
                weight = 14
            }
        };

    private DogsHouseDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DogsHouseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DogsHouseDbContext(options);
    }



    [Fact]
    public async Task Dogs_Return200()
    {

        var mockService = new Mock<IDogsService>();
        mockService.Setup(s => s.GetSortedAndPaginatedDogs(null, null, 1, 10)).ReturnsAsync(data.AsQueryable());
        var dogsController = new DogsController(mockService.Object);
        var result = await dogsController.Dogs();
        result.Should().NotBeNull();

    }

    [Fact]
    public async Task Dogs_ForwardsParameters()
    {
        var mock = new Mock<IDogsService>();
        mock.Setup(s => s.GetSortedAndPaginatedDogs("name", "desc", 2, 5))
            .ReturnsAsync(data.AsQueryable());

        var controller = new DogsController(mock.Object);
        await controller.Dogs(attribute: "name", order: "desc", pageNumber: 2, pageSize: 5);
        mock.Verify(s => s.GetSortedAndPaginatedDogs("name", "desc", 2, 5), Times.Once);
    }
    [Fact]
    public async Task Dogs_SortsAndPaginates_Returns200()
    {
        var options = new DbContextOptionsBuilder<DogsHouseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new DogsHouseDbContext(options);
        var repo = new DogsRepository(context);

        foreach (Dog d in data)
        {
            await repo.CreateAsync(d);
        }
        await repo.SaveChangesAsync();

        var mockUnit = new Mock<IUnitOfWork>();
        mockUnit.Setup(u => u.Dogs).Returns(repo);

        var service = new DogsService(mockUnit.Object);

        var result = await service.GetSortedAndPaginatedDogs("name", "asc", 1, 10);

        mockUnit.Verify(r => r.Dogs, Times.AtLeastOnce);
        result.Should().NotBeNull();
        result.Should().BeInAscendingOrder(d => d.name);
    }

    [Fact]
    public async Task Dogs_WithWrongAttribute_Returns200()
    {
        await using var context = CreateInMemoryContext();
        var repo = new DogsRepository(context);

        foreach (Dog d in data)
        {
            await repo.CreateAsync(d);
        }
        await repo.SaveChangesAsync();

        var mockUnit = new Mock<IUnitOfWork>();
        mockUnit.Setup(u => u.Dogs).Returns(repo);

        var service = new DogsService(mockUnit.Object);

        var result = await service.GetSortedAndPaginatedDogs("jump_heigth", "asc", 1, 10);

        mockUnit.Verify(r => r.Dogs, Times.AtLeastOnce);
        result.Should().NotBeNull();
    }





    [Fact]
    public async Task DogsController_CreateDog_Returns201AndCallsService()
    {
        // Arrange
        var mockService = new Mock<IDogsService>();
        mockService.Setup(s => s.CreateDog(It.IsAny<Dog>())).Returns(Task.CompletedTask).Verifiable();

        var controller = new DogsController(mockService.Object);
        var dog = new Dog { name = "NewDog", color = "black & white", tail_length = (short)4, weight = (short)8 };

        // Act
        var result = await controller.CreateDog(dog);

        // Assert
        var status = result.Should().BeOfType<Microsoft.AspNetCore.Mvc.ObjectResult>().Subject;
        status.StatusCode.Should().Be(201);
        mockService.Verify(s => s.CreateDog(It.Is<Dog>(d =>
            d.name == dog.name &&
            d.color == dog.color &&
            d.tail_length == dog.tail_length &&
            d.weight == dog.weight)), Times.Once);
    }

    [Fact]
    public async Task DogsController_CreateDog_InvalidModel_ReturnsBadRequest()
    {
        var mockService = new Mock<IDogsService>();

        mockService.Setup(s => s.CreateDog(It.IsAny<Dog>())).Throws(new ArgumentException());

        var controller = new DogsController(mockService.Object);

        var dog = new Dog { name = "x", tail_length = (short)1, weight = (short)1 };

        var result = await controller.CreateDog(dog);

        result.Should().BeOfType<BadRequestObjectResult>();
        mockService.Verify(s => s.CreateDog(It.IsAny<Dog>()), Times.Once);
    }

    [Fact]
    public void Dog_DogModelValidation()
    {
        // Tail length zero
        var d1 = new Dog { name = "A", color = "black", tail_length = (short)0, weight = (short)1 };
        var results1 = new List<ValidationResult>();
        var ok1 = Validator.TryValidateObject(d1, new ValidationContext(d1), results1, true);
        ok1.Should().BeFalse();
        results1.SelectMany(r => r.MemberNames).Should().Contain("tail_length");

        // Weight zero
        var d2 = new Dog { name = "B", color = "black", tail_length = (short)1, weight = (short)0 };
        var results2 = new List<ValidationResult>();
        var ok2 = Validator.TryValidateObject(d2, new ValidationContext(d2), results2, true);
        ok2.Should().BeFalse();
        results2.SelectMany(r => r.MemberNames).Should().Contain("weight");

        // Color not correct 
        var d3 = new Dog { name = "B", color = "black1", tail_length = (short)1, weight = (short)1 };
        var results3 = new List<ValidationResult>();
        var ok3 = Validator.TryValidateObject(d3, new ValidationContext(d3), results2, true);
        ok3.Should().BeFalse();
        results2.SelectMany(r => r.MemberNames).Should().Contain("color");

    }
}