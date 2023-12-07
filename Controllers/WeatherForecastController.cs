using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StokTakipWebApi12B.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
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
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public string Kimsin()
        {
            return "Mustafa";
        }

        [HttpGet]
        public string OgrenciListesiGetir()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(liste);
        }

        [HttpPost]
        public string OgrenciBul(int id)
        {
            Ogrenci ogr = liste.FirstOrDefault(x => x.Id == id);

            return JsonConvert.SerializeObject(ogr);
        }

        static Ogrenci[] liste =
        {
            new Ogrenci(){Id=1,Ad="Emirhan", Soyad="Kurban",Sinif="12B"},
            new Ogrenci(){Id=2,Ad="Kaan", Soyad="Salman",Sinif="12B"},
            new Ogrenci(){Id=3,Ad="Eyüp", Soyad="Sezgin",Sinif="12B"},
            new Ogrenci(){Id=4,Ad="Adem", Soyad="Sözüçok",Sinif="12B"},
            new Ogrenci(){Id=5,Ad="Doðukan", Soyad="Kurban",Sinif="12B"},
            new Ogrenci(){Id=6,Ad="Umut", Soyad="Kahraman",Sinif="12B"},
            new Ogrenci(){Id=7,Ad="Muhammed", Soyad="Kaplan",Sinif="12B"},
        };
        class Ogrenci
        {
            public int Id { get; set; }
            public string Ad { get; set; }
            public string Soyad { get; set; }
            public string Sinif { get; set; }
        }

    }
}