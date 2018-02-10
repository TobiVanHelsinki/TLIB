using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TLIB_UWPFRAME.Resources
{
    public class Helper
    {
        public static IEnumerable<PropertyInfo> GetProperties(object obj, Type type)
        {
            //var t = obj.GetType().GetProperties();
            return obj.GetType().GetProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }


    }
}
