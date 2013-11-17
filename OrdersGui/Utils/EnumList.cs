using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hylasoft.OrdersGui.Utils
{
    public class EnumList
    {
        /// <summary>
        /// Lists all the enumeration values for a give enum type
        /// </summary>
        /// <typeparam name="T">The enum to list</typeparam>
        /// <returns>The list of enum values of the given enum type</returns>
        public static List<T> GetEnumValues<T>()
        {
            var t = typeof(T);
            var enumValues = new List<T>();
            if (!t.IsEnum)
            {
                throw new ArgumentException("The template type should be an enum");
            }
            var fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields.Select(fi => (T)Enum.Parse(t, fi.Name, false)).ToList();
        }
    }
}
