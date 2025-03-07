using Microsoft.AspNetCore.Mvc;

namespace ToDoWeb.Controllers
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

        // POST
        // PUT : Idempotency là khi gọi nhiều lần cũng như nhau 
        // Khác biệt của PUT và POST là 1 đứa là upate, 1 đứa là tạo mới
        // Muốn update thì xài PUT chứ post thì nếu call nhiều lần sẽ bị duplicate trong database 
        // Front end ->>>>>>>>>> Back end, Backend created data
        // Front end <<<<<<<<<-- Backend 


        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return new List<WeatherForecast>
            {
                new WeatherForecast
                {
                    Date = DateTime.Now,
                    TemperatureC = 32,
                    Summary = "hot"
                }
            };
        }
    }
}
