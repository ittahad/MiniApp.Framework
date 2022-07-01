using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public class RabbitMqConsumerOptions
    {
        public string? ListenOnQueue { get; set; } 
        public string? ListenViaExchange { get; set; } 
        public int? PrefetchCount { get; set; } 
    }
}
