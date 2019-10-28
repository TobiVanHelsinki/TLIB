//Author: Tobi van Helsinki

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
        /// Obsolete
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [Obsolete]
        public static IEnumerable<MethodInfo> GetMethods(object obj, Type type)
        {
            return obj.GetType().GetRuntimeMethods().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }

        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [Obsolete]
        public static IEnumerable<PropertyInfo> GetProperties(object obj, Type type)
        {
            return obj.GetType().GetRuntimeProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }

        /// <summary>
        /// returns a collection of PropertyInfos, which have the given attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(Type type, Type attributeType)
        {
            return type.GetRuntimeProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == attributeType));
        }

        /// <summary>
        /// returns a collection of PropertyInfos, which have the given attribute
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(object obj, Type attributeType)
        {
            return GetPropertiesWithAttribute(obj.GetType(), attributeType);
        }

        /// <summary>
        /// returns a collection of MethodInfos, which have the given attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type type, Type attributeType)
        {
            return type.GetRuntimeMethods().Where(p => p.CustomAttributes.Any(c => c.AttributeType == attributeType));
        }

        /// <summary>
        /// returns a collection of MethodInfos, which have the given attribute
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(object obj, Type attributeType)
        {
            return GetMethodsWithAttribute(obj.GetType(), attributeType);
        }

        /// <summary>
        /// Get all CustomAttributes with type TAttribute from this EnumValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static IEnumerable<TAttribute> GetCustomAttributesFromEnum<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>();
        }
    }
}