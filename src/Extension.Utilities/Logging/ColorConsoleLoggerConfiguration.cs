using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extension.Utilities.Logging
{
    public class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }
        public LogLevel MinLogLevel { get; set; } = LogLevel.Information;
    }
}
