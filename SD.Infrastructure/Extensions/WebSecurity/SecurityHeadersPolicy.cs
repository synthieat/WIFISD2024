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
        /// A dictionary of Header, Value pairs that should be added to all requests
        /// </summary>
        public IDictionary<string, string> SetHeaders { get; } = new Dictionary<string, string>();

        /// <summary>
        /// A hashset of Headers that should be removed from all requests
        /// </summary>
        public ISet<string> RemoveHeaders { get; } = new HashSet<string>();

        public ICollection<string> ExcludePathsStartsWith { get; } = new List<string>();
    }
}
