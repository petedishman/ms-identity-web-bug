using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;

namespace IntegrationTests;

public class UnitTest4 : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;

    public UnitTest4(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Fails in both .net 6 & 7 but for different reasons.
    /// In .net 6 fails for same reason as SingleRequestTriggersOneCallToMicrosoftIdentityOptionsConfiguration
    ///     i.e. the configuration callback is always called multiple times
    /// In .net 7 multiple concurrent requests after app startup will trigger multiple calls to
    /// the configuration function, more than the 2 done by .net 6
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task MultipleRequestsTriggerOneCallToMicrosoftIdentityOptionsConfiguration()
    {
        // Arrange
        var client = _factory.CreateClient();
        var requestCount = 10;

        // Act
        var requestTasks = Enumerable.Range(1, requestCount).Select(_ => client.GetAsync("/"));
        var responses = await Task.WhenAll(requestTasks);

        // Assert
        Assert.Equal(1, CallCounts.MsIdentityOptionsConfiguration);
    }
}
