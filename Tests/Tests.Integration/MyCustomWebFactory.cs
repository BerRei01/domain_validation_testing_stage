using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Tests.Integration;
public class MyCustomWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Override services if needed (e.g., replace with test database)
        });

        builder.ConfigureTestServices(services =>
        {
            // Additional test-specific configurations
        });
    }
}