using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Infrastructure.Extensions.WebSecurity
{
    public class SecurityHeadersPolicy
    {
        /// <summary>
        /// Dictionary um Wertepare (Policies) dem Header hinzuzufügen
        /// </summary>
        public IDictionary<string, string> SetHeaders { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Ist eine Liste von Headers, die aus dem HTTP Header entfernt werden sollen
        /// </summary>
        public ISet<string> RemoveHeaders { get; } = new HashSet<string>();


        public ICollection<string> ExcludePathsStartsWith { get; } = new HashSet<string>();
    }
}
