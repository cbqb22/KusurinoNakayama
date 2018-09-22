using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OASystem.Model.Validator
{
    public class JanCodeValidatorRule : ValidationRule
    {

        public JanCodeValidatorRule()
        {
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            var Spattern = @"\d{13}"; // S群
            var Smatches = Regex.Matches(value.ToString(), Spattern);

            if (value.ToString().Length != 13 || Smatches.Count == 0)
            {
                return new ValidationResult(false, "13桁の数字を入力");
            }


            return new ValidationResult(true, null);
        }

    }
}
