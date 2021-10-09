using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiYi.IoT.SDK;

namespace DiYi.IoT.Sample.Controllers
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

       

        private readonly IIoTPublisher _iotBus;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IIoTPublisher capPublisher)
        {
            _iotBus = capPublisher;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {

            _iotBus.PublishAsync("test", "123");

            return Content("12");


        }
    }
}
