using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;

namespace IntegrationTests;

public class UnitTest3 : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;

    public UnitTest3(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Passes in both .net 6 & 7
    /// A single request coming in after app startup triggers just one call to the
    /// RedirectToIdentityProvider callback
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SingleRequestTriggersOneCallToRedirectToIdProvider()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        await client.GetAsync("/");

        // Assert
        Assert.Equal(1, CallCounts.RedirectToIdProvider);
    }
}
