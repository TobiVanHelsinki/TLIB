using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TLIB
{
    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetMethods(object obj, Type type)
        {
            //return obj.GetType().GetProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
            return obj.GetType().GetRuntimeProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }

        public static IEnumerable<PropertyInfo> GetProperties(object obj, Type type)
        {
            return obj.GetType().GetRuntimeProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }

        public static IEnumerable<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetRuntimeProperties();
        }
    }
}
