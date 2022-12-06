using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;

namespace IntegrationTests;

public class UnitTest2 : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;

    public UnitTest2(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }


    /// <summary>
    /// This passes in both dotnet 6 & 7
    /// The passed in callback is called only once as expected
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SingleRequestTriggersOneCallToClientApplicationOptionsConfiguration()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        await client.GetAsync("/");

        // Assert
        Assert.Equal(1, CallCounts.ClientAppOptionsConfiguration);
    }
}
