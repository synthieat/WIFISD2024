using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using System.Resources;
using SD.Rescources;
using SD.Rescources.Attributes;

public abstract class BaseTestsWithDI
{
    protected static IServiceProvider Services;
    protected CancellationToken CancellationToken { get; private set; }
    protected abstract IConfigurationRoot GetConfigurations();
    protected abstract void RegisterServices(ServiceCollection serviceCollection, IConfigurationRoot configuration);
    private static ServiceCollection serviceCollection = new ServiceCollection();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        CancellationToken = new CancellationTokenSource().Token;
        var deCulture = new CultureInfo("de");
        CultureInfo.CurrentCulture = deCulture;
        CultureInfo.CurrentUICulture = deCulture;


        // Create an instance of ResourceManager
        ResourceManager resourceManager = new ResourceManager(typeof(BasicRes));

        // Register the ResourceManager instance with the IServiceCollection
        serviceCollection.AddSingleton(resourceManager);
        LocalizedDescriptionAttribute.Setup(resourceManager);


        serviceCollection.Clear();
        var configuration = GetConfigurations();
        RegisterServices(serviceCollection, configuration);


        serviceCollection.AddLogging();

        BaseTestsWithDI.Services = serviceCollection.BuildServiceProvider();

    }

}