using System;
using System.ComponentModel.DataAnnotations;

namespace RssSE.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true )]
    public class ExpirationCardAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
                return false;
            var month = value.ToString().Split('/')[0];
            var year = $"20{value.ToString().Split('/')[1]}";

            if(int.TryParse(month, out int mon)
                && int.TryParse(year, out int y))
            {
                var date = new DateTime(y, mon, 1);
                return date > DateTime.UtcNow;
            }

            return false;
        }
    }
}
