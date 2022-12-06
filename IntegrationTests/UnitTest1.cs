using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;
using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]

namespace IntegrationTests;
public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;

    public UnitTest1(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// This currently fails in both dotnet 6 & 7 so isn't a change of behaviour but is
    /// confusing and undocumented?
    /// 
    /// Both .AddMicrosoftIdentityWebApp() and .EnableTokenAcquisitionToCallDownstreamApi()
    /// register the same configuration callback with the service collection causing it
    /// to always be called twice
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SingleRequestTriggersOneCallToMicrosoftIdentityOptionsConfiguration()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        await client.GetAsync("/");

        // Assert
        Assert.Equal(1, CallCounts.MsIdentityOptionsConfiguration);
    }
}
