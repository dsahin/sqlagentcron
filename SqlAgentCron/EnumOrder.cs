using System;
using System.Reflection;

namespace SqlAgentCron
{
    public class EnumOrder<TEnum> where TEnum : struct
    {
        private static readonly TEnum[] Values;

        static EnumOrder()
        {
            var fields = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            Values = Array.ConvertAll(fields, x => (TEnum)x.GetValue(null));
        }

        public static int IndexOf(TEnum value)
        {
            return Array.IndexOf(Values, value);
        }

        public static TEnum ValueAt(int index)
        {
            return Values[index];
        }
    }
}