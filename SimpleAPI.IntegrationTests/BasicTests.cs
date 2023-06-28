using System.Data.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleAPI.Data;

namespace SimpleAPI.IntegrationTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient Client;
    
    public BasicTests()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationDbContext)); //Remove App's real DB Context

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                    
                    //https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
                    // var dbContextDescriptor = services.SingleOrDefault(
                    //     d => d.ServiceType ==
                    //          typeof(DbContextOptions<ApplicationDbContext>));
                    //
                    // services.Remove(dbContextDescriptor);
                    //
                    // var dbConnectionDescriptor = services.SingleOrDefault(
                    //     d => d.ServiceType ==
                    //          typeof(DbConnection));
                    //
                    // services.Remove(dbConnectionDescriptor);
                    //
                    // // Create open SqliteConnection so EF won't automatically close it.
                    // services.TryAddSingleton<DbConnection>(container =>
                    // {
                    //     var connection = new SqliteConnection("DataSource=:memory:");
                    //     connection.Open();
                    //
                    //     return connection;
                    // });
                    //
                    // services.AddDbContext<ApplicationDbContext>((container, options) =>
                    // {
                    //     var connection = container.GetRequiredService<DbConnection>();
                    //     options.UseSqlite(connection);
                    // });
                });

                builder.UseEnvironment("Development");
            });
        Client = _factory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtToken());
    }

    protected async Task<IEnumerable<WeatherForecast>?> CreateWeatherForecast(params WeatherForecast[] forecast)
    {
        var response = await Client.PostAsJsonAsync("WeatherForecast/CreateWeatherForecast", forecast);
        return await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
    }

    private async Task<string> GetJwtToken()
    {
        //assuming this is the endpoint to send user credentials and get token
        var response = await Client.PostAsJsonAsync("WeatherForecast/Auth", new UserAuthRequest
        {
            Email = "user@app.com",
            Password = "asdASD123"
        });

        var authResponse = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();
        return authResponse.Token;
    }
}