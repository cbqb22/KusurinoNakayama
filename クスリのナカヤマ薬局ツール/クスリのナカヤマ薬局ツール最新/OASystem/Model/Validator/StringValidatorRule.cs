using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace OASystem.Model.Validator
{
    public class StringValidatorRule : ValidationRule
    {

        private int _minlength;
        private int _maxlength;


        public int MinLength
        {
            get { return _minlength; }
            set { _minlength = value; }
        }

        public int MaxLength
        {
            get { return _maxlength; }
            set { _maxlength = value; }
        }

        public StringValidatorRule()
        {
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "文字を入力して下さい。");
            }

            var str = value.ToString();

            if (str.Length < MinLength)
            {
                return new ValidationResult(false, string.Format("{0}文字以上で入力",MinLength));
            }
            else if (MaxLength < str.Length)
            {
                return new ValidationResult(false, string.Format("{0}文字以内で入力", MinLength));
            }

            return new ValidationResult(true, null);
        }

    }
}
