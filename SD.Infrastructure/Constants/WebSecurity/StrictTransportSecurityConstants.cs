using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.Constants.WebSecurity
{
    public static class StrictTransportSecurityConstants
    {
        /// <summary>
        /// Header für Strict-Transport-Security
        /// </summary>
        public static readonly string Header = "Strict-Transport-Security";

        /// <summary>
        /// Dauer wie lange der URL im STS gecacht wird. Angabe in Sekunden.
        /// </summary>
        public static readonly string MaxAge = "max-age={0}";

        /// <summary>
        /// URL wird aus dem STS cache entfernt
        /// </summary>
        public static readonly string NoCache = "max-age=0";


    }
}
