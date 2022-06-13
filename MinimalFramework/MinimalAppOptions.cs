using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public class MinimalAppOptions
    {
        public string[]? CommandLineArgs { get; set; }
        public string? StartUrl { get; set; }
        public bool? UseSwagger { get; set; }
        public bool? ConsoleLogging { get; set; }
        public bool? FileLogging { get; set; }
        public SeqLoggerOptions SeqLoggerOptions { get; set; }
    }

    public class SeqLoggerOptions
    {
        public bool? UseSeq { get; set; }
        public string? SeqServerUrl { get; set; }
    }
}
