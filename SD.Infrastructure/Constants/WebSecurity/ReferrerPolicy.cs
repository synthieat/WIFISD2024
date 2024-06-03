using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.Constants.WebSecurity
{
    public static class ReferrerPolicyConstants
    {
        /// <summary>
        /// The header value for Referrer-Policy
        /// </summary>
        public static readonly string Header = "Referrer-Policy";

        public static readonly string NoReferrer = "no-referrer";

        public static readonly string StrictOriginWhenCrossOrigin = "strict-origin-when-cross-origin";
    }
}
