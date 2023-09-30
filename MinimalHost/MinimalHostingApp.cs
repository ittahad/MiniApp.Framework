
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using MiniApp.Core;
using System.Reflection;

namespace MinimalHost
{
    public class MinimalHostingApp
    {
        public IHost Host { get; set; }
        private readonly MinimalHostOptions _options;

        public MinimalHostingApp(MinimalHostOptions options = null)
        {
            _options = options == null ? new MinimalWebAppOptions()
            {
                CommandLineArgs = new string[] { },
            } : options;
        }

        public void Run()
        {
            Host.Run();
        }
    }
}