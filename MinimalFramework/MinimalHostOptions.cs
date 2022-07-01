using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public class MinimalHostOptions
    {
        public string[]? CommandLineArgs { get; set; }
        public bool? ConsoleLogging { get; set; }
        public bool? FileLogging { get; set; }
        public LoggingProviders LoggingProvider { get; set; } = LoggingProviders.Default;
        public SeqLoggerOptions SeqLoggerOptions { get; set; }
    }

    public class SeqLoggerOptions
    {
        public bool? UseSeq { get; set; }
        public string? SeqServerUrl { get; set; }
    }

    public enum LoggingProviders { 
        Serilog,
        Default,
    }

}
