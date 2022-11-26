using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Vista.Pages.Validaciones.ValidacionesUsuario
{
    public class NombreIsValid : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var nombre = Convert.ToString(value);

                if (nombre != null && nombre != string.Empty)
                {
                    if (!NotContainsSpecialChars(nombre))
                    {
                        return new ValidationResult(false, "El nombre no puede contener caracteres especiales");
                    }
                    if (nombre.Length >= 60)
                    {
                        return new ValidationResult(false, "El nombre no puede superar los 60 caracteres");
                    }
                }
                else
                {
                    return new ValidationResult(false, "El nombre es requerido");
                }
                return ValidationResult.ValidResult;
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Algo anda mal");
            }

            static bool NotContainsSpecialChars(string s)
            {
                Regex regex = new(@"^[a-zA-Z\s]*$");
                return regex.IsMatch(s);
            }            
        }
    }
}
