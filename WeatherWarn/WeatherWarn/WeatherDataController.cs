using OpenMeteo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace WeatherWarn
{
    public class WeatherDataController : IWeatherDataController
    {
        private OpenMeteoClient client = new();

        public WeatherData FetchWeatherData()
        {
            var forecast = client.Query("Trois-Rivières");

            if (forecast == null || forecast.Current == null)
                throw new InvalidOperationException("Could not fetch weather data from open-meteo.");

            return new WeatherData {
                Rain = forecast.Current.Rain ?? 0,
                Snow = forecast.Current.Snowfall ?? 0,
                Temperature = forecast.Current.Temperature ?? 0
            };
        }
    }
}
