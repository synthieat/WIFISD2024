using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace SD.Infrastructure.Extensions.WebSecurity
{
    public static class ContentSecurityServiceExtensions
    {
        private const string HEADER_X_REQUESTED_WITH = "X-Requested-With";

        public static void AddSecurityHeaders(this IServiceCollection services, string[] allowedCspOrigins = null, string[] allowedCorsOrigins = null, string[] excludePathsStartsWith = null)
        {
            var securityHeadersBuilder = new SecurityHeadersBuilder()
                .AddContentSecurePolicy(allowedCspOrigins)
                .AddFrameOptionsSameOrigin() // needed for silent-renew in SPA
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyStrictOriginWhenCrossOrigin()
                .RemoveServerHeader()

                // this headers should be set to prevent steal information from browsers cache
                .AddCustomHeader("Pragma", "no-cache")
                .AddCustomHeader("Cache-control", "no-cache,no-store")

                .ExcludePathsStartsWith(excludePathsStartsWith);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                // securityHeadersBuilder.AddStrictTransportSecurityMaxAgeIncludeSubDomains();
            }

            services.AddSingleton(securityHeadersBuilder.Build());

            if (allowedCorsOrigins != null && allowedCorsOrigins.Count() > 0)
            {
                // only enable CORS policy if the resources will be used (API request or framing content) by a defined origin
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder => builder.WithOrigins(allowedCorsOrigins)
                                                               .AllowCredentials() // allow send auth cookies from allowed applications
                                                               .WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete)
                                                               .WithHeaders(HeaderNames.Authorization, HEADER_X_REQUESTED_WITH) // jquery sends 'x-requested-with'
                                                               .SetPreflightMaxAge(TimeSpan.FromHours(6))
                    );
                });
            }
        }
    }
}
