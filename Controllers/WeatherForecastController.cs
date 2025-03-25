using Microsoft.AspNetCore.Mvc;

namespace WebHookWhatsApp.Controllers
{
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

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet (Name = "TesteMocado")]
        public IActionResult GetMockData()
        {
            var mockData = new List<object>
        {
            new { Id = 1, Nome = "Matheus", Email = "matheus@email.com" },
            new { Id = 2, Nome = "João", Email = "joao@email.com" },
            new { Id = 3, Nome = "Maria", Email = "maria@email.com" }
        };

            return Ok(mockData);
        }
    }
}
