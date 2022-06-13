using Microsoft.AspNetCore.Builder;
using MinimalFramework;

namespace MinimalWebApi
{
    public class MinimalWebApp
    {
        public WebApplication? Application;

        private MinimalAppOptions _options;
        
        public MinimalWebApp(MinimalAppOptions options = null)
        {
            _options = options == null ? new MinimalAppOptions() { 
                UseSwagger = false,
                CommandLineArgs = new string[] { },
            } : options;
        }

        public void Start()
        {
            if (Application == null) throw new Exception();
            Application.UseRouting();
            Application.UseAuthentication();
            Application.UseAuthorization();
            
            Application.UseMvc(config =>
            {
                string serviceName = Application.Configuration["ServiceName"];
                config.MapRoute(
                    name: "default",
                    template: serviceName + "/{controller}/{action}/{id?}");
            });

            if (_options.UseSwagger.HasValue && _options.UseSwagger.Value)
            {
                Application
                    .UseSwagger()
                    .UseSwaggerUI();
            }
            
            if (_options.StartUrl != null)
            {
                Application.Urls.Add(_options.StartUrl);
            }

            Application.Run();
        }

        public void Start(Action<WebApplication> application)
        {
            application.Invoke(Application);
            Start();
        }

    }
}
