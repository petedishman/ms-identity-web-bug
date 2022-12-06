using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;

namespace IntegrationTests;

public class UnitTest6 : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;

    public UnitTest6(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Passes in .net 6, fails in .net 7
    /// Multiple concurrent requests coming in after app startup should
    /// trigger one call to the RedirectToIdentityProvider callback for each request
    /// instead, we get multiple calls per incoming request in .net 7
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task MultipleRequestsTriggerMatchingCallsToRedirectToIdProvider()
    {
        // Arrange
        var client = _factory.CreateClient();
        var requestCount = 10;

        // Act
        var requestTasks = Enumerable.Range(1, requestCount).Select(_ => client.GetAsync("/"));
        var responses = await Task.WhenAll(requestTasks);

        // Assert
        Assert.Equal(requestCount, CallCounts.RedirectToIdProvider);
    }


}