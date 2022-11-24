using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Vista.Pages.Validaciones.ValidacionesUsuario
{
    public class EmailIsValid : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {                  
                var email = Convert.ToString(value);
                if (email != null && email != string.Empty)
                {
                    if (!IsValidEmailAddress(email))
                    {
                        return new ValidationResult(false, "Debe tener formato correo");                        
                    }
                    if (email.Length > 254)
                    {
                        return new ValidationResult(false, "El email no puede superar los 254 caracteres");
                    }
                }
                else
                {
                    return new ValidationResult(false, "El email es requerido");
                }
                return ValidationResult.ValidResult;
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Algo anda mal");
            }

            static bool IsValidEmailAddress(string s)
            {
                Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                return regex.IsMatch(s);
            }
        }
    }
}