using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWarn;

namespace WeatherWarnTests
{
    class LoggerMock : ILogger
    {
        public string LastLoggedLine { get; private set; }

        public void LogLine(string line)
        {
            LastLoggedLine = line;
        }
    }
}
