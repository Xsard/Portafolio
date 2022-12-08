using Controlador;
using Modelo;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vista.Pages.Validaciones;

namespace Vista.Pages
{
    public partial class MantenedorAdmin : Page
    {
        bool rutValido = false;
        bool passValida = false;
        public MantenedorAdmin()
        {
            InitializeComponent();
            ListarAdmin();
        }
        private void ItemError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }
        private void ListarAdmin()
        {
            try
            {
                DataTable dataTable = CAdmin.ListarAdmin();
                if (dataTable != null)
                {
                    var admin = (from rw in dataTable.AsEnumerable()
                                 select new Administrador()
                                 {
                                     IdUsuario = Convert.ToInt32(rw[0]),
                                     Rut = rw[8].ToString(),
                                     Nombres = rw[9].ToString(),
                                     Apellidos = rw[10].ToString(),
                                     Email = rw[1].ToString(),
                                     Telefono = Convert.ToInt32(rw[3])
                                 }).ToList();
                    dtgAdmin.ItemsSource = admin;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_Agregar_Admin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validarForm())
                {
                    string pattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
                    if (txt_pass_ag.Password.Length > 8 && txt_pass_ag.Password.Length < 30)
                    {
                        if (!Regex.IsMatch(txt_pass_ag.Password, pattern) || txt_pass_ag.Password != txt_passConfirm_ag.Password)
                        {
                            MessageBox.Show("Las contraseñas no coinciden o no son lo suficientemente seguras");
                            return;
                        }
                        string nRut = txt_rut_ag.Text.Split('-').First();
                        string dvRut = txt_rut_ag.Text.Split('-').Last();
                        if (!Rut.ValidaRut(nRut, dvRut))
                        {
                            MessageBox.Show("El rut ingresado no es válido");
                            return;
                        }
                        pattern = "^\\S+@\\S+\\.\\S+$";
                        if (!Regex.IsMatch(txt_email_ag.Text, pattern))
                        {
                            MessageBox.Show("Ingrese un correo con formato válido");
                            return;
                        }
                        Administrador userAdmin = new()
                        {
                            Email = txt_email_ag.Text.Trim(),
                            Contraseña = txt_pass_ag.Password.Trim(),
                            Telefono = Convert.ToInt32(txt_fono_ag.Text.Trim()),
                            Rut = txt_rut_ag.Text.Trim(),
                            Nombres = txt_nombres_ag.Text.Trim(),
                            Apellidos = txt_apellidos_ag.Text.Trim(),
                        };

                        int estado = CAdmin.CrearUsuarioAdmin(userAdmin);
                        MensajeOk("Administrador agregado");
                        ListarAdmin();
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show("La contraseña debe tener entre 8 y 30 caracteres");                        
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.StackTrace);
            }
        }
        private bool validarForm()
        {
            try
            {
                if (txt_rut_ag.Text != string.Empty)
                {
                    if (txt_nombres_ag.Text != string.Empty)
                    {
                        if (txt_apellidos_ag.Text != string.Empty)
                        {
                            if (txt_fono_ag.Text != string.Empty)
                            {
                                if (txt_email_ag.Text != string.Empty)
                                {
                                    if (txt_pass_ag.Password != string.Empty)
                                    {
                                        if (txt_passConfirm_ag.Password != string.Empty)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            MensajeError("La confirmación de contraseña es requerida");
                                        }
                                    }
                                    else
                                    {
                                        MensajeError("La contraseña es requerida");
                                    }
                                }
                                else
                                {
                                    MensajeError("El correo es requerido");
                                }
                            }
                            else
                            {
                                MensajeError("El telefono es requerido");

                            }
                        }
                        else
                        {
                            MensajeError("Los apellidos son requeridos");
                        }
                    }
                    else
                    {
                        MensajeError("Los nombres son requeridos");
                    }
                }
                else
                {
                    MensajeError("El RUT es requerido");
                }
                return false;   
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void DtgAdminUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Administrador userAdmin = (Administrador)dtgAdmin.SelectedItem;
                    try
                    {
                        int estado = CAdmin.ActualizarAdmin(userAdmin);
                        MensajeOk("Administrador actualizado");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.StackTrace);
            }
        }
        private void DtgAdminDelete_Click(object sender, RoutedEventArgs e)
        {
            Administrador admin = (Administrador)dtgAdmin.SelectedItem;
            try
            {
                MessageBoxResult result = MessageBox.Show("Estás seguro de querer eliminar este usuario administrador?", "Administrador", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int estado = CAdmin.EliminarAdmin(admin.IdUsuario);
                    MensajeOk("Administrador eliminado");
                    ListarAdmin();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Limpiar()
        {
            txt_email_ag.Clear();
            txt_pass_ag.Clear();
            txt_passConfirm_ag.Clear();
            txt_fono_ag.Clear();
            txt_rut_ag.Clear();
            txt_nombres_ag.Clear();
            txt_apellidos_ag.Clear();
        }
        private void btnAbrirAgregarAdmin_Click(object sender, RoutedEventArgs e)
        {
            dhAdmin_ag.IsOpen = true;
        }
        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhAdmin_ag.IsOpen = false;
        }       
        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Administrador", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MensajeOk(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Administrador", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void txt_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú0-9\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void txt_fono_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void txt_rut_ag_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_rut_ag.Text.Length >= 2)
            {
                txt_rut_ag.Text = txt_rut_ag.Text.ToString().Insert(txt_rut_ag.Text.Length - 1, "-");
            }
        }

        private void txt_rut_ag_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_rut_ag.Text = txt_rut_ag.Text.Replace("-", "");
        }

        private void txt_rut_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9k\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Telefono_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Apellidos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Nombres_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                    [0-9]{1,2}|25[0-5]|2[0-4][0-9])\." + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                    [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            Regex regex = new Regex(pattern);
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_nombres_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_apellidos_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_email_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                    [0-9]{1,2}|25[0-5]|2[0-4][0-9])\." + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                    [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            Regex regex = new Regex(pattern);
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
