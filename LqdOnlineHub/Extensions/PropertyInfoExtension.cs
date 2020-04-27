using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LqdOnlineHub.Extensions
{
    public static class PropertyInfoExtension
    {
        public static string GetDisplayName(this PropertyInfo prop)
        {
            var displayAttr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .Cast<DisplayNameAttribute>();

            if (displayAttr.Count() == 0) return prop.Name;
            else return displayAttr.Single().DisplayName;
        }

        public static string EnumGetDisplayName(this PropertyInfo prop, object obj)
        {
            if (!prop.PropertyType.IsEnum) throw new InvalidOperationException();

            var enumValue = prop.GetValue(obj);
            var name = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();

            return name;
        }
    }
}
