# dotnet 7 bug(s) in Microsoft.Identity.Web

- SDK: NET 7.0.100
- Microsoft.Identity.Web: 1.25.10

## How to run
```sh
dotnet test
```

## Bug
With dotnet 7, concurrent requests coming in to an aspnetcore app just after startup trigger Microsoft.Identity.Web configuration 
methods to run multiple times. 

Handlers on the authentication events objects are then also called multiple times.

This is a change since the introduction of dotnet 7.

Another issue, is that by using both `AddMicrosoftIdentityWebApp()` and `EnableTokenAcquisitionToCallDownstreamApi()` the configuration 
callback passed to `AddMicrosoftIdentityWebApp()` is always called twice. This applies to both .net 6 and .net 7
