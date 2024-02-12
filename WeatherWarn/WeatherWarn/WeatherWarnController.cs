using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWarn
{
    public class WeatherWarnController
    {
        private IWeatherDataController weatherDataController;
        private ILogger logger;

        public WeatherWarnController(IWeatherDataController weatherDataController, ILogger logger)
        {
            this.weatherDataController = weatherDataController;
            this.logger = logger;
        }

        public void ValidateWeather()
        {
            var data = weatherDataController.FetchWeatherData();

            if (data.Rain > 8)
                logger.LogLine("Avertissement: Pluie violente");
            if (data.Snow > 20)
                logger.LogLine("Avertissement: Tempête de neige");
            if (data.Temperature > 40)
                logger.LogLine("Avertissement: Canicule");
        }
    }
}
