using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nolan.AutoClicker.Views.ValidationRules
{
    public class NumberOnlyRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public NumberOnlyRule()
        {
            this.Min = int.MinValue;
            this.Max = int.MaxValue;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number = 0;

            try
            {
                if (((string)value).Length > 0)
                {
                    number = int.Parse((string)value);
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, $"Only number accepted.");
            }

            if ((number < this.Min) || (number > Max))
            {
                return new ValidationResult(false,
                  $"Out of range: {this.Min}-{this.Max}");
            }
            return ValidationResult.ValidResult;
        }
    }

}
