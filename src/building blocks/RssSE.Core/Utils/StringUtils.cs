using System.Linq;

namespace RssSE.Core.Utils
{
    public static class StringUtils
    {
        public static string OnlyNumbers(this string str, string input) => new string(input.Where(x => char.IsLetter(x)).ToArray());
    }
}
