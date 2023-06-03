using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingHost
{
    public static class TracingProvider
    {
        public static ActivitySource MyActivitySource = new ActivitySource("MassTransit");
    }
}
