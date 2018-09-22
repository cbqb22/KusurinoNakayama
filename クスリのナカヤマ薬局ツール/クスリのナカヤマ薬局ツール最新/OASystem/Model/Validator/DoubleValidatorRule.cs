using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace OASystem.Model.Validator
{
    public class DoubleValidatorRule : ValidationRule
    {

        private double _min;
        private double _max;


        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }


        public DoubleValidatorRule()
        {
            _min = double.MinValue;
            _max = double.MaxValue;
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double result;
            if (double.TryParse(value.ToString(), out result) == false)
            {
                return new ValidationResult(false, "数字を入力");
            }

            if (result < Min)
            {
                return new ValidationResult(false, string.Format("{0}以上で入力", Min));
            }
            else if (Max < result)
            {
                return new ValidationResult(false, string.Format("{0}以内で入力", Max));
            }

            return new ValidationResult(true, null);
        }

    }
}
