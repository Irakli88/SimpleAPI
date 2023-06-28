using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace SimpleAPI.IntegrationTests;

public class ControllerTests : BasicTests
{
    [Fact]
    public async void Test1()
    {
        var response = await Client.GetAsync("WeatherForecast/GetWeatherForecast");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            
    }
    
    [Fact]
    public async void GetWeathers_With_Auth()
    {
        // Arrange
        await AuthenticateAsync(); //if Token Auth exists

        // Act
        var response = await Client.GetAsync("WeatherForecast/GetEmptyWeatherForecast");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<WeatherForecast>>()).Should().BeEmpty();
    }
    
    [Fact]
    public async void CreateWeather_With_Auth()
    {
        // Arrange
        await AuthenticateAsync(); //if Token Auth exists
        var weatherForecast = new WeatherForecast
        {
            Date = new DateOnly(2020, 3, 19),
            TemperatureC = 23,
            Summary = "Chilly"
        };

        // Act
        var response = await CreateWeatherForecast(weatherForecast);
        
        // Assert
        response.Should().NotBeEmpty();
        response.Count().Should().Be(1);
        var forecast = response.ToList()[0];
        forecast.TemperatureC.Should().Be(23);

    }
}