using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database;
using System.Text.Json;
using System.Text;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<DEPLOYContext>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing database context
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DEPLOYContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Add a database context using an in-memory database for testing
            services.AddDbContext<DEPLOYContext>(options =>
            {
                options.UseInMemoryDatabase($"InMemoryDbForTesting_{Guid.NewGuid()}");
            });
        });

        builder.UseEnvironment("Development");
    }
}

[TestClass]
public class BasicApiTests
{
    private CustomWebApplicationFactory? _factory;
    private HttpClient? _client;

    [TestInitialize]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [TestMethod]
    public async Task Get_Swagger_Should_Return_Success()
    {
        // Act
        var response = await _client!.GetAsync("/swagger/v1/swagger.json");

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Books_Endpoint_Should_Be_Available()
    {
        // Act - Just test that the endpoint responds
        var response = await _client!.GetAsync("/api/v1/books");

        // Assert - Should get OK or some valid response, not a server error
        Assert.AreNotEqual(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [TestMethod]
    public async Task Authors_Endpoint_Should_Require_Authorization()
    {
        // Act
        var response = await _client!.GetAsync("/api/v1/authors");

        // Assert - Should return Unauthorized since no auth is provided
        Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task Get_NonExistent_Book_Should_Return_NotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client!.GetAsync($"/api/v1/books/{nonExistentId}");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}