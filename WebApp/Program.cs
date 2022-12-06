using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        IdentityModelEventSource.ShowPII = true;

        var builder = WebApplication.CreateBuilder(args);

        CallCounts.Reset();

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(
                msIdentityOptions => {
                    // this is microsoftIdentityOptionsConfiguration callback in tests

                    builder.Configuration.GetSection("AzureAd").Bind(msIdentityOptions);

                    CallCounts.MsIdentityOptionsConfiguration++;
                    Console.WriteLine("Configure ms identity options (call {0})", CallCounts.MsIdentityOptionsConfiguration);

                    msIdentityOptions.Events.OnRedirectToIdentityProvider = context => { 
                        CallCounts.RedirectToIdProvider++;
                        Console.WriteLine("OnRedirectToIdentityProvider called (call {0})", CallCounts.RedirectToIdProvider);
                        return Task.CompletedTask; 
                    };
                })
            .EnableTokenAcquisitionToCallDownstreamApi(clientAppOptions => {
                // this is clientApplicationOptionsConfiguration callback in tests

                CallCounts.ClientAppOptionsConfiguration++;
                Console.WriteLine("Configure client app options (call {0})", CallCounts.ClientAppOptionsConfiguration);
            })
            .AddInMemoryTokenCaches();

        builder.Services.AddAuthorization(options =>
            options.AddPolicy("Auth", policyBuilder => policyBuilder.RequireAuthenticatedUser()));

        var app = builder.Build();

        app.UseDeveloperExceptionPage();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Hello World!").RequireAuthorization("Auth");

        app.Run();
    }
}

