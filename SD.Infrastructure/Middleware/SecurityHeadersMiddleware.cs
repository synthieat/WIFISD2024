using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SD.Infrastructure.Constants.WebSecurity;
using SD.Infrastructure.Extensions.WebSecurity;

namespace SD.Infrastructure.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private const string LOCALHOST = "localhost";

        private readonly RequestDelegate _next;
        private readonly SecurityHeadersPolicy _policy;

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

            this._next = next;
            this._policy = policy;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof (context));
            }

            var response = context.Response;

            if(response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if(this._policy.ExcludePathsStartsWith.Count > 0 && _policy.ExcludePathsStartsWith.Any(c => context.Request.Path.StartsWithSegments(c)))
            {
                await _next(context);
                return;
            }

            var headers = response.Headers;

            foreach(var headerValuePair in _policy.SetHeaders)
            {
                headers[headerValuePair.Key] = headerValuePair.Value;
            }

            foreach(var removeHeader in _policy.RemoveHeaders)
            {
                headers.Remove(removeHeader);
            }

            if(context.Request.Host.Host == LOCALHOST)
            {
                headers.Remove(StrictTransportSecurityConstants.Header);
            }

            await _next(context);

        }
    
    }
}
