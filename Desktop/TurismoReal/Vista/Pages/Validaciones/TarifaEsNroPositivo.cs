using System;
using System.Globalization;
using System.Windows.Controls;

namespace Vista.Pages.Validaciones
{
    public class TarifaEsNroPositivo : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var numero = Convert.ToInt32(value);

                if (numero <= 0)
                {
                    return new ValidationResult(false, "La tarifa debe ser un número positivo");
                }
                return ValidationResult.ValidResult;
            }
            catch (Exception)
            {
                return new ValidationResult(false, "La tarifa debe ser un número");
            }
        }
    }
}
