using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TLIB
{
    /// <summary>
    /// Methods providing short access for recurring tasks involving reflection
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// returns MethodInfos of the type of obj, that have type as Attributes
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethods(object obj, Type type)
        {
            return obj.GetType().GetRuntimeMethods().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }

        /// <summary>
        /// returns PropertyInfos of the type of obj, that have type as Attributes
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties(object obj, Type type)
        {
            return obj.GetType().GetRuntimeProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }
    }
}
