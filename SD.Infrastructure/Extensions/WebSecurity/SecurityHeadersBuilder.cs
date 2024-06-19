using Microsoft.Extensions.Primitives;
using SD.Infrastructure.Constants.WebSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.Extensions.WebSecurity
{
    /// <summary>
    /// Exposes methods to build a policy.
    /// </summary>
    public class SecurityHeadersBuilder
    {
        private readonly SecurityHeadersPolicy _policy = new SecurityHeadersPolicy();

        /// <summary>
        /// The number of seconds in one year
        /// </summary>
        public const int OneYearInSeconds = 60 * 60 * 24 * 365;

        /// <summary>
        /// Add default headers in accordance with most secure approach
        /// </summary>
        public SecurityHeadersBuilder AddDefaultSecurePolicy()
        {
            AddFrameOptionsDeny();
            AddXssProtectionBlock();
            AddContentTypeOptionsNoSniff();
            AddStrictTransportSecurityMaxAge();
            RemoveServerHeader();

            return this;
        }

        /// <summary>
        /// Add Referrer-Policy: Send the origin, path, and querystring when performing a
        /// same-origin request, only send the origin when the protocol security level
        /// stays the same while performing a cross-origin request (HTTPS > HTTPS), and
        /// send no header to any less-secure destinations (HTTPS > HTTP).
        /// </summary>
        /// <returns></returns>
        public SecurityHeadersBuilder AddReferrerPolicyStrictOriginWhenCrossOrigin()
        {
            _policy.SetHeaders[ReferrerPolicyConstants.Header] = ReferrerPolicyConstants.StrictOriginWhenCrossOrigin;
            return this;
        }

        /// <summary>
        /// Add X-Frame-Options DENY to all requests.
        /// The page cannot be displayed in a frame, regardless of the site attempting to do so
        /// </summary>
        public SecurityHeadersBuilder AddFrameOptionsDeny()
        {
            _policy.SetHeaders[FrameOptionsConstants.Header] = FrameOptionsConstants.Deny;
            return this;
        }

        /// <summary>
        /// Add X-Frame-Options SAMEORIGIN to all requests.
        /// The page can only be displayed in a frame on the same origin as the page itself.
        /// </summary>
        public SecurityHeadersBuilder AddFrameOptionsSameOrigin()
        {
            _policy.SetHeaders[FrameOptionsConstants.Header] = FrameOptionsConstants.SameOrigin;
            return this;
        }

        /// <summary>
        /// Add X-Frame-Options ALLOW-FROM {uri} to all requests, where the uri is provided
        /// The page can only be displayed in a frame on the specified origin.
        /// </summary>
        /// <param name="uri">The uri of the origin in which the page may be displayed in a frame</param>
        public SecurityHeadersBuilder AddFrameOptionsSameOrigin(string uri)
        {
            _policy.SetHeaders[FrameOptionsConstants.Header] = string.Format(FrameOptionsConstants.AllowFromUri, uri);
            return this;
        }


        /// <summary>
        /// Add X-XSS-Protection 1 to all requests.
        /// Enables the XSS Protections
        /// </summary>
        public SecurityHeadersBuilder AddXssProtectionEnabled()
        {
            _policy.SetHeaders[XssProtectionConstants.Header] = XssProtectionConstants.Enabled;
            return this;
        }

        /// <summary>
        /// Add X-XSS-Protection 0 to all requests.
        /// Disables the XSS Protections offered by the user-agent.
        /// </summary>
        public SecurityHeadersBuilder AddXssProtectionDisabled()
        {
            _policy.SetHeaders[XssProtectionConstants.Header] = XssProtectionConstants.Disabled;
            return this;
        }

        /// <summary>
        /// Add X-XSS-Protection 1; mode=block to all requests.
        /// Enables XSS protections and instructs the user-agent to block the response in the event that script has been inserted from user input, instead of sanitizing.
        /// </summary>
        public SecurityHeadersBuilder AddXssProtectionBlock()
        {
            _policy.SetHeaders[XssProtectionConstants.Header] = XssProtectionConstants.Block;
            return this;
        }

        /// <summary>
        /// Send Content-Security-Policy header
        /// </summary>
        /// <param name="allowedOrigins"></param>
        /// <returns></returns>
        public SecurityHeadersBuilder AddContentSecurePolicy(ICollection<string> allowedOrigins)
        {
            string allowedOriginsString = allowedOrigins != null ? ' ' + string.Join(' ', allowedOrigins.Where(w => !string.IsNullOrEmpty(w)).Distinct()) : string.Empty;

#pragma warning disable CS8601 // Possible null reference assignment.
            _policy.SetHeaders[ContentSecurityPolicyConstants.Header] = new StringValues(
                $"default-src 'self' {allowedOriginsString};" +
                "script-src 'self';" +
                "style-src 'self';" +
                "font-src 'self';" +
                $"img-src 'self' data: {allowedOriginsString};" +
                "form-action 'self';" +
                "frame-ancestors 'self';" + // allow framing of our applications itself (needed for SPA renew token)
                "object-src 'none';" +
                "media-src 'none';" +
                "manifest-src 'self';" +
                "worker-src 'self';"
            );
#pragma warning restore CS8601 // Possible null reference assignment.

            return this;
        }

        /// <summary>
        /// Add X-XSS-Protection 1; report=http://site.com/report to all requests.
        /// A partially supported directive that tells the user-agent to report potential XSS attacks to a single URL. Data will be POST'd to the report URL in JSON format.
        /// </summary>
        public SecurityHeadersBuilder AddXssProtectionReport(string reportUrl)
        {
            _policy.SetHeaders[XssProtectionConstants.Header] =
                string.Format(XssProtectionConstants.Report, reportUrl);
            return this;
        }

        /// <summary>
        /// Add Strict-Transport-Security max-age=<see cref="maxAge"/> to all requests.
        /// Tells the user-agent to cache the domain in the STS list for the number of seconds provided.
        /// </summary>
        public SecurityHeadersBuilder AddStrictTransportSecurityMaxAge(int maxAge = OneYearInSeconds)
        {
            _policy.SetHeaders[StrictTransportSecurityConstants.Header] =
                string.Format(StrictTransportSecurityConstants.MaxAge, maxAge);
            return this;
        }

        
        /// <summary>
        /// Add Strict-Transport-Security max-age=0 to all requests.
        /// Tells the user-agent to remove, or not cache the host in the STS cache
        /// </summary>
        public SecurityHeadersBuilder AddStrictTransportSecurityNoCache()
        {
            _policy.SetHeaders[StrictTransportSecurityConstants.Header] =
                StrictTransportSecurityConstants.NoCache;
            return this;
        }

        /// <summary>
        /// Add X-Content-Type-Options nosniff to all requests.
        /// Can be set to protect against MIME type confusion attacks.
        /// </summary>
        public SecurityHeadersBuilder AddContentTypeOptionsNoSniff()
        {
            _policy.SetHeaders[ContentTypeOptionsConstants.Header] = ContentTypeOptionsConstants.NoSniff;
            return this;
        }

        /// <summary>
        /// Removes the Server header from all responses
        /// </summary>
        public SecurityHeadersBuilder RemoveServerHeader()
        {
            _policy.RemoveHeaders.Add(ServerConstants.Header);
            return this;
        }

        /// <summary>
        /// Adds a custom header to all requests
        /// </summary>
        /// <param name="header">The header name</param>
        /// <param name="value">The value for the header</param>
        /// <returns></returns>
        public SecurityHeadersBuilder AddCustomHeader(string header, string value)
        {
            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentNullException(nameof(header));
            }

            _policy.SetHeaders[header] = value;
            return this;
        }

        /// <summary>
        /// Remove a header from all requests
        /// </summary>
        /// <param name="header">The to remove</param>
        /// <returns></returns>
        public SecurityHeadersBuilder RemoveHeader(string header)
        {
            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentNullException(nameof(header));
            }

            _policy.RemoveHeaders.Add(header);
            return this;
        }

        public SecurityHeadersBuilder ExcludePathsStartsWith(params string[] paths)
        {
            if (paths == null)
            {
                return this;
            }

            foreach (var path in paths)
            {
                _policy.ExcludePathsStartsWith.Add(path);
            }

            return this;
        }

        /// <summary>
        /// Builds a new <see cref="SecurityHeadersPolicy"/> using the entries added.
        /// </summary>
        /// <returns>The constructed <see cref="SecurityHeadersPolicy"/>.</returns>
        public SecurityHeadersPolicy Build()
        {
            return _policy;
        }
    }
}
