using System.Threading.Tasks;
using DogsHouse.Controllers;
using FluentAssertions;


namespace DogsHouse.Tests.Controller;

public class HomeControllerTests
{
    [Fact]
    public async Task HomeController_Ping_ReturnOk()
    {
        var homeController = new HomeController();
        var result = await homeController.Ping();
        result.Should().NotBeNull();
    }
}

