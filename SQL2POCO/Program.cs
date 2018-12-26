using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SQL2POCO
{

    
    public class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void ConfigureServices(ServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json");
            IConfiguration configuration = configBuilder.Build();
            services.AddSingleton(configuration);
            services.AddSingleton(log);
        }
        static void Main(string[] args)
        {
            //CompareUnitTest.Test1();
            //return;
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            var configRecs = new List<POCOConfig>();
            configuration.GetSection("POCOConfig").Bind(configRecs);
            foreach (var config in configRecs)
            {
                SQL2PocoGen.Generate(ServiceProvider, config);

            }
        }
     
    }
}
