using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWarn;

namespace WeatherWarnTests
{
    class WeatherDataControllerMock : IWeatherDataController
    {
        public float Rain { get; set; } = 0;
        public float Snow { get; set; } = 0;
        public float Temperature { get; set; } = 0;

        public WeatherData FetchWeatherData()
        {
            return new WeatherData
            {
                Rain = Rain,
                Snow = Snow,
                Temperature = Temperature
            };
        }
    }
}
