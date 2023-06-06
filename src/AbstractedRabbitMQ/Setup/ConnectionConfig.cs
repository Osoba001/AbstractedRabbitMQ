using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractedRabbitMQ.Setup
{
    public sealed class ConnectionConfig
    {
        public ConnectionConfig()
        {
            this.Url = string.Empty;
        }
        public string Url { get; set; }
        public string? ClientProvideName { get; set; }
    }
}
