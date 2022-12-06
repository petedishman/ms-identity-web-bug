using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;

namespace IntegrationTests;

public class UnitTest5 : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;

    public UnitTest5(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// This passes in .net 6 & fails in .net 7
    /// Multiple concurrent requests coming in after app startup trigger multiple calls
    /// to the client application configuration callback.
    /// In .net 6 you just get the one call as expected
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task MultipleRequestsTriggerOneCallToClientApplicationOptionsConfiguration()
    {
        // Arrange
        var client = _factory.CreateClient();
        var requestCount = 10;

        // Act
        var requestTasks = Enumerable.Range(1, requestCount).Select(_ => client.GetAsync("/"));
        var responses = await Task.WhenAll(requestTasks);

        // Assert
        Assert.Equal(1, CallCounts.ClientAppOptionsConfiguration);
    }
}
