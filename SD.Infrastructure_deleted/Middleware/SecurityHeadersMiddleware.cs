using Microsoft.AspNetCore.Http;
using SD.Infrastructure.Constants.WebSecurity;
using SD.Infrastructure.Extensions.WebSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private const string LOCALHOST = "localhost";
        private readonly RequestDelegate _next;
        private readonly SecurityHeadersPolicy _policy;

        /// <summary>
        /// Instantiates a new <see cref="SecurityHeadersMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="policy">An instance of the <see cref="SecurityHeadersPolicy"/> which can be applied.</param>
        public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersPolicy policy)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            _next = next;
            _policy = policy;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.Response;

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            // exclude request paths if configured
            if (_policy.ExcludePathsStartsWith.Count > 0 && _policy.ExcludePathsStartsWith.Any(c => context.Request.Path.StartsWithSegments(c)))
            {
                await _next(context);
                return;
            }

            var headers = response.Headers;

            foreach (var headerValuePair in _policy.SetHeaders)
            {
                headers[headerValuePair.Key] = headerValuePair.Value;
            }

            foreach (var header in _policy.RemoveHeaders)
            {
                headers.Remove(header);
            }

            if (context.Request.Host.Host == LOCALHOST)
            {
                // never send HSTS header on localhost environment to avoid HTTP issues on local machines
                headers.Remove(StrictTransportSecurityConstants.Header);
            }

            await _next(context);
        }
    }
}
