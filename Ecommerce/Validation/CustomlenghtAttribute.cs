using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Validation
{
    public class CustomlenghtAttribute: ValidationAttribute
    {
        private int _min;
        private int _max;
        public CustomlenghtAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }
        public override bool IsValid(object? value)
        {
            if(value is string str)
            {
                if(str.Length >= _min && str.Length <= _max)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }
        //public override string FormatErrorMessage(string name)
        //{
        //    return $"The field {name} must be between {_min} and {_max} characters long.";
        //}
    }
}
