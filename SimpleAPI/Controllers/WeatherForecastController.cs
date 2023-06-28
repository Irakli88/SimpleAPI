using Microsoft.AspNetCore.Mvc;

namespace SimpleAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet]
    [Route("GetEmptyWeatherForecast")]
    public IEnumerable<WeatherForecast> GetEmpty()
    {
        return Enumerable.Empty<WeatherForecast>();
    }
    [HttpPost]
    [Route("PostWeatherForecast")]
    public IEnumerable<WeatherForecast> Post(int minTmp, int maxTmp)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(minTmp, maxTmp),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpPost]
    [Route("CreateWeatherForecast")]
    public IEnumerable<WeatherForecast> Post(WeatherForecast[] forecast)
    {
        var result = new List<WeatherForecast>();
        result.AddRange(forecast);
        return result;
    }
    
    [HttpPost]
    [Route("Auth")]
    public AuthSuccessResponse Post(UserAuthRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new Exception("Invalid Credentials");
        }
        
        return new AuthSuccessResponse
        {
            Token = Guid.NewGuid().ToString().Replace("-", "")
        };
    }
}