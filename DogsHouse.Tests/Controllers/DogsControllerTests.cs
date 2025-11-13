

namespace DogsHouse.Tests.Controller;

public class DogsControllerTests
{
    public async Task Dogs_Return200()
    {
        var data = new List<Dog>
        {
            new Dog
            {
                name = "Neo",
                color = "black & white",
                tail_length = 32,
                weight = 22
            }
        };
        var mockService = new Mock<IDogsService>();
        var dogsController = new DogsController(mockService.Object);
        var result = await dogsController.Dogs();
        result.Should().NotBeNull();
    }
}


