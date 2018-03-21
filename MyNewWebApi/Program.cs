using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyNewWebAPi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            BuildHost(args).Run();
        }


        public static IWebHost BuildHost(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((Action<WebHostBuilderContext, IConfigurationBuilder>)
                        ((hostingContext, config) =>
                {
                    IHostingEnvironment hostingEnvironment = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile(string.Format("appsettings.{0}.json", (object)hostingEnvironment.EnvironmentName), true, true);

                    if (hostingEnvironment.IsDevelopment())
                    {
                        Assembly assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                        if (assembly != (Assembly)null)
                            config.AddUserSecrets(assembly, true);
                    }

                    config.AddEnvironmentVariables();
                    if (args == null)
                        return;
                    config.AddCommandLine(args);
                }))
                .ConfigureLogging((Action<WebHostBuilderContext, ILoggingBuilder>)((hostingContext, logging) =>
                {
                    logging.AddConfiguration((IConfiguration)hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                }))
                .UseIISIntegration()
                .UseDefaultServiceProvider((Action<WebHostBuilderContext, ServiceProviderOptions>)
                    ((context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment()))
                .UseStartup<Startup>()
                .UseUrls(new string[]{"http://localhost:5000"})
                
                .Build();

            ;
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
