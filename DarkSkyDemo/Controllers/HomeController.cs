using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DarkSkyDemo.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace DarkSkyDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _weatherUrl = "https://api.darksky.net/forecast/";
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var weather = await GetWeatherAsync();
            return Ok(weather);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<Weather> GetWeatherAsync()
        {
            var key = _config["ApiKeys:DarkSky"];
            var url = $"{_weatherUrl}{key}/36,-86";
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var weather = await response.Content.ReadAsAsync<Weather>();
                return weather;
            }
            else
            {
                return null;
            }
        }
    }
}
