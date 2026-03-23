using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mouts.CrossCutting.Utils.Extensions;
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name == null) return value.ToString();
        var field = type.GetField(name);
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();

        return attr?.Description ?? name;
    }

    public static bool IsInEnum<TEnum>(this int value) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }
}
