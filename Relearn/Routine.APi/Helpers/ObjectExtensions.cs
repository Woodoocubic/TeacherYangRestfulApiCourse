using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Routine.APi.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject shapeData<TSource>(this TSource source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var expandoObj = new ExpandoObject();
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.IgnoreCase
                                   | BindingFlags.Public | BindingFlags.Instance);
                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandoObj).Add(propertyInfo.Name, propertyValue);
                }
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(
                    propertyName, BindingFlags.IgnoreCase 
                                  | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        throw new Exception($"cannot find {propertyName} at {typeof(TSource)}");
                    }

                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandoObj).Add(propertyInfo.Name, propertyValue);
                }
            }

            return expandoObj;
        }
    }
}
 