using System.ComponentModel;

namespace HotTowel.Core.Api.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var desc = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return desc.Length > 0 ? desc[0].Description : value.ToString();
        }

        public static List<string> GetDescriptions(this Enum value)
        {
            var type = value.GetType();
            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds) descs.Add(fd.Description);
            }

            return descs;
        }

        public static T GetEnum<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.ToLower().Trim() == description.ToLower().Trim()) return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name.ToLower().Trim() == description.ToLower().Trim()) return (T)field.GetValue(null);
                }
            }

            return default;
        }

        public static T GetEnumValue<T>(this string value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (field.Name.ToLower().Trim() == value.ToLower().Trim()) return (T)field.GetValue(null);
            }

            return default;
        }
    }
}