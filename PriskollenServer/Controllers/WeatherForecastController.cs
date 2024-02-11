using Microsoft.AspNetCore.Mvc;

namespace PriskollenServer.Controllers;
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

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("You requested a weather report");

        try
        {
            for (int i = 0; i < 100; i++)
            {
                if (i == 56)
                {
                    throw new ArgumentException("This is our demo exception");
                }
                else
                {
                    _logger.LogInformation("Weather forecasts for {WeatherId}", i);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "We caught an exception!");
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
