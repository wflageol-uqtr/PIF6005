using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherWarn;

namespace WeatherWarnTests
{
    [TestClass]
    public class WeatherWarnControllerTests
    {
        private WeatherWarnController controller;
        private WeatherDataControllerMock wdcMock;
        private LoggerMock loggerMock;

        [TestInitialize]
        public void Setup()
        {
            wdcMock = new WeatherDataControllerMock();
            loggerMock = new LoggerMock();
            controller = new WeatherWarnController(wdcMock, loggerMock);
        }

        [TestMethod]
        public void TestHeavyRain()
        {
            // Arrange
            wdcMock.Rain = 15;

            // Act
            controller.ValidateWeather();

            // Assert
            StringAssert.Contains(
                "Avertissement: Pluie violente",
                loggerMock.LastLoggedLine);
        }

        [TestMethod]
        public void TestNotHeavyRain()
        {
            // Arrange
            wdcMock.Rain = 2;

            // Act
            controller.ValidateWeather();

            // Assert
            Assert.IsNull(loggerMock.LastLoggedLine);
        }
    }
}
