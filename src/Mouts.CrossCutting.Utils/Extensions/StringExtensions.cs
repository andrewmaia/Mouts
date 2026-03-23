using System.Text;
using System.Text.RegularExpressions;


namespace Mouts.CrossCutting.Utils.Extensions;
public static class StringExtensions
{
    public static string ToSnakeCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        var snake = Regex.Replace(str, @"([a-z0-9])([A-Z])", "$1_$2");
        return snake.ToLower();
    }

    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        var words = str.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        foreach (var word in words)
        {
            sb.Append(char.ToUpper(word[0]) + word.Substring(1).ToLower());
        }
        return sb.ToString();
    }

    public static string OnlyNumbers(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return Regex.Replace(str, @"\D", "");
    }

    public static string ToCnpjMask(this string cnpj)
    {
        if (string.IsNullOrEmpty(cnpj)) return cnpj;

        string onlyNumbers = Regex.Replace(cnpj, @"\D", "");

        if (onlyNumbers.Length != 14)
            return cnpj; 

        return Convert.ToUInt64(onlyNumbers).ToString(@"00\.000\.000\/0000\-00");
    }

    public static string ToCpfMask(this string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) return cpf;

        string onlyNumbers = Regex.Replace(cpf, @"\D", "");

        if (onlyNumbers.Length != 11) return cpf;

        return Convert.ToUInt64(onlyNumbers).ToString(@"000\.000\.000\-00");
    }
}
