
using TaskManagementService.CommonLib.Enums;

namespace TaskManagementService.CommonLib.CustomAttributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class OrderingDescriptionAttribute : Attribute
    {
        public string Field;
        public OrderingEnum Ordering;

        public OrderingDescriptionAttribute(string field, OrderingEnum ordering)
        {
            Field = field;
            Ordering = ordering;
        }
    }
}
