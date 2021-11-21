using HotTowel.Core.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class WeatherForecasterrController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecasterrController> _logger;

        public WeatherForecasterrController(ILogger<WeatherForecasterrController> logger)
        {
            _logger = logger;
        }

        [HttpGet(ApiRoutes.BedInfo, Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Gett()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}