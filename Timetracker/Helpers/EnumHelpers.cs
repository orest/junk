using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Timetracker.Entities.Models.Base;

namespace MyProjects.Helpers
{
    public static class EnumHelpers
    {
        public static IEnumerable<SelectListItem> GetItems(
            this Type enumType, int? selectedValue)
        {
            if (!typeof(Enum).IsAssignableFrom(enumType))
            {
                throw new ArgumentException("Type must be an enum");
            }

            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType).Cast<int>();

            var items = names.Zip(values, (name, value) =>
                    new SelectListItem
                    {
                        Text = GetName(enumType, name),
                        Value = value.ToString(),
                        Selected = value == selectedValue
                    }
                );
            return items;
        }


        static string GetName(Type enumType, string name)
        {
            var result = name;

            var attribute = enumType
                .GetField(name)
                .GetCustomAttributes(inherit: false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            if (attribute != null)
            {
                result = attribute.GetName();
            }

            return result;
        }

        public static T ToEnum<T>(this string strEnumValue, T defaultValue)
        {
            if (!Enum.IsDefined(typeof(T), strEnumValue))
                return defaultValue;

            return (T)Enum.Parse(typeof(T), strEnumValue);
        }


        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> source) where T : LookupBase
        {
            return source.Select(item => new SelectListItem()
            {
                Value = item.Id.HasValue ? item.Id.ToString() : item.Code,
                Text = item.Description
            }).ToList();
        }


        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static class SiteCookies
        {
            public const string ActiveProfile = "apfid";
        }
        public static class CustomClaims
        {
            public const string UserId = "UserId";
        }
    }
}