using Controlador;
using Modelo;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Vista.Pages
{
    public partial class MantenedorServExtras : Page
    {
        public MantenedorServExtras()
        {
            InitializeComponent();
            ListarSvE();
        }
        private void ItemError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }
        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Servicios Extra", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MensajeOk(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Servicios Extra", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #region Agregar
        private void btnAbrirAgregarServ_Click(object sender, RoutedEventArgs e)
        {
            dhServ_ag.IsOpen = true;
        }
        private void btn_Agregar_Servicio_Click(object sender, RoutedEventArgs e)
        {
            if (!txt_nombre_ag.Text.Trim().Equals(""))
            {
                if (!txt_desc_ag.Text.Trim().Equals(""))
                {
                    if (int.TryParse(txt_precio_ag.Text, out int precio))
                    {
                        try
                        {
                            ServicioExtra servicioExtra = new()
                            {
                                NombreServicioExtra = txt_nombre_ag.Text,
                                DescripcionServicioExtra = txt_desc_ag.Text,
                                ValorServicioExtra = precio
                            };
                            int estado = CServicioExtra.IngresarServicio(servicioExtra);
                            MensajeOk("Servicio extra agregado");
                            Limpiar();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }
        private void Limpiar()
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Refresh();
        }

        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhServ_ag.IsOpen = false;
            Limpiar();
        }
        #endregion
        #region Listar
        private void ListarSvE()
        {
            try
            {
                DataTable dataTable = CServicioExtra.ListarServicios();
                if (dataTable != null)
                {
                    var Serv = (from rw in dataTable.AsEnumerable()
                                select new ServicioExtra()
                                {
                                    IdServicioExtra = Convert.ToInt32(rw[0]),
                                    NombreServicioExtra = rw[1].ToString(),
                                    DescripcionServicioExtra = rw[2].ToString(),
                                    ValorServicioExtra = Convert.ToInt32(rw[3])
                                }).ToList();
                    dtgServE.ItemsSource = Serv;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        private void DtgServDelete(object sender, RoutedEventArgs e)
        {
            ServicioExtra servicioExtra = (ServicioExtra)dtgServE.SelectedItem;
            try
            {
                int estado = CServicioExtra.EliminarServicio(servicioExtra.IdServicioExtra);
                MensajeOk("Servicio eliminado");
                ListarSvE();
            }
            catch (Exception)
            {
            }
        }

        private void txt_string_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_int_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        ServicioExtra? servEActualizar;
        private void dtgServE_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            servEActualizar = (ServicioExtra)dtgServE.SelectedItem;
            if (servEActualizar == null) return;
            dhServ_ac.IsOpen = true;
            txt_nombre_ac.Text = servEActualizar.NombreServicioExtra;
            txt_desc_ac.Text = servEActualizar.DescripcionServicioExtra;
            txt_precio_ac.Text = servEActualizar.ValorServicioExtra.ToString();
        }

        private void btn_Actualizar_Servicio_Click(object sender, RoutedEventArgs e)
        {
            servEActualizar.NombreServicioExtra = txt_nombre_ac.Text;
            servEActualizar.DescripcionServicioExtra = txt_desc_ac.Text;
            servEActualizar.ValorServicioExtra = int.Parse(txt_precio_ac.Text);
            int estado = CServicioExtra.ActualizarServicio(servEActualizar);
            if (estado > 0)
            {
                MensajeOk("Servicio extra actualizado");
                Limpiar();
            }
            dhServ_ac.IsOpen = false;
            servEActualizar = null;
        }

        private void btn_Cancelar_Ac_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();

        }
    }
}
