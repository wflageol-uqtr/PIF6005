using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWarn
{
    public interface ILogger
    {
        void LogLine(string line);
        void LogFormat(string format, params string[] args)
        {
            LogLine(string.Format(format, args));
        }
    }
}
