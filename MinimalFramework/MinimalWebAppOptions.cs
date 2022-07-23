using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public class MinimalWebAppOptions : MinimalHostOptions
    {
        public string? StartUrl { get; set; }
        public bool? UseSwagger { get; set; }
        public bool? UseAuthentication { get; set; } = false;
    }
}
