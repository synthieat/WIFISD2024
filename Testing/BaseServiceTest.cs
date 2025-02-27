using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SD.Persistence.Repositories.DBContext;
using SD.Persistence.Extensions;
using static Testing.BaseServiceTest;
using NUnit.Framework;
using SD.Application.Extensions;

namespace Testing
{
    public class BaseServiceTests<TService> : BaseServiceTest
    {
        protected TService Service;


        [OneTimeSetUp]
        public void ServiceSetup()
        {
            this.Service = BaseTestsWithDI.Services.GetRequiredService<TService>();
        }

    }


    public class BaseServiceTest: BaseTestsWithDI
    {
        private const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

        protected override IConfigurationRoot GetConfigurations()
        {
            var sharedFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "Shared");
            var env = Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT);


            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("sharedsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"sharedsettings.{env}.json", optional: true, reloadOnChange: true)
                 .AddJsonFile(Path.Combine(sharedFolder, "sharedsettings.json"), optional: true, reloadOnChange: true)
                 .AddJsonFile(Path.Combine(sharedFolder, $"sharedsettings.{env}.json"), optional: true, reloadOnChange: true)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables();

            var config = configurationBuilder.Build();            
            return config;
        }

        protected override void RegisterServices(ServiceCollection serviceCollection, IConfigurationRoot configuration)
        {
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            var conn = configuration.GetConnectionString("MovieDbContext");
            serviceCollection.AddDbContext<MovieDbContext>(options => options.UseSqlServer(conn));

            serviceCollection.RegisterRepositories();
            serviceCollection.RegisterApplicationService();
        }   
    }
}
