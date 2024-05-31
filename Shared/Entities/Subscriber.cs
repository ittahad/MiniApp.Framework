using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Subscriber
    {
        public Guid Id { get; set; }         
        public string Email { get; set; }
        public DateTime SubscribedOnUtc { get; set; }
    }
}
