using System.ComponentModel;
using System.Globalization;
using TaskManagementService.CommonLib.CustomAttributes;

namespace TaskManagementService.CommonLib.Helpers;

public static class EnumHelpers
{
    public static T? GetValueFromDescription<T>(string description) where T : struct
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (string.Compare(
                    attribute.Description,
                    description,
                    CultureInfo.CurrentCulture,
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (string.Compare(
                    field.Name,
                    description,
                    CultureInfo.CurrentCulture,
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                    return (T)field.GetValue(null);
            }
        }

        return default;
    }

    public static string GetDescription(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (field == null)
            return enumValue.ToString();

        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
        {
            return attribute.Description;
        }

        return enumValue.ToString();
    }

    public static string GetOrdering(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (field == null)
            return enumValue.ToString();

        if (Attribute.GetCustomAttribute(field, typeof(OrderingDescriptionAttribute)) is OrderingDescriptionAttribute attribute)
        {
            var description = attribute.Ordering.GetDescription();
            string ordering;

            if (!string.IsNullOrWhiteSpace(description))
                ordering = description;
            else
                ordering = enumValue.ToString();

            return $"{attribute.Field} {ordering}";
        }

        return enumValue.ToString();
    }
}
