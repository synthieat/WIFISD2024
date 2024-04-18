using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Base
{
    public abstract class BaseHandler
    {
        protected void MapEntityProperties<TSource, TTarget>(TSource source, TTarget target, List<string> excludeProperties = default)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            if(sourceType.BaseType.FullName != targetType.BaseType.FullName)
            {
                throw new ApplicationException("Base types are not matching!");
            }

            var targetPropertyInfos = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            targetPropertyInfos.ForEach(p =>
            {
                if(p.CanWrite && !(excludeProperties ?? []).Contains(p.Name))
                {
                    /* Passendes Property aus Quelle lesen */
                    var sourceProperty = sourceType.GetProperty(p.Name, BindingFlags.Instance | BindingFlags.Public);
                    if(sourceProperty != null)
                    {
                        /* Wert aus Quelle auslesen */
                        var sourcePropertyValue = sourceProperty.GetValue(source, null);
                        /* Ausgelsener Wert ins Target-Property schreiben */
                        p.SetValue(target, sourcePropertyValue);
                    }
                }
            });

        }
        
    }
}
