using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.DataAnnotations
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;

            var comparisonValue = (DateTime?)validationContext.ObjectType.GetProperty(_comparisonProperty)
                                                                        .GetValue(validationContext.ObjectInstance);

            if (currentValue is not null && currentValue <= comparisonValue)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = FormatErrorMessage(validationContext.DisplayName);
                }

                return new ValidationResult(ErrorMessage, new[] {validationContext.MemberName});
            }

            return ValidationResult.Success;
        }
    }
}
