using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SD.Core.Attributes;

namespace SD.Application.Extensions
{
    public static class ServiceBuilderExtension
    {
        public static void RegisterApplicationService(this IServiceCollection services)
        {
            /* Scrutor Extension .Scan */
            services.Scan(scan =>
                scan.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(c => c.WithAttribute<MapServiceDependencyAttribute>())
                    .AsSelf()
                    .WithScopedLifetime());

        }
    }
}
