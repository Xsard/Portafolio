using Controlador;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vista.Pages
{
    /// <summary>
    /// Interaction logic for MantenedorServExtras.xaml
    /// </summary>
    public partial class MantenedorServExtras : Page
    {
        public MantenedorServExtras()
        {
            InitializeComponent();
            ListarSvE();
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
                            MessageBox.Show("Servicio agregado");
                            ListarSvE();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
        }

        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhServ_ag.IsOpen = false;
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
        private void DtgServUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ServicioExtra servicioExtra = (ServicioExtra)dtgServE.SelectedItem;
                try
                {
                    int estado = CServicioExtra.ActualizarServicio(servicioExtra);
                    MessageBox.Show("Servicio actualizado");
                    ListarSvE();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private void DtgServDelete(object sender, RoutedEventArgs e)
        {
            ServicioExtra servicioExtra = (ServicioExtra)dtgServE.SelectedItem;
            try
            {
                int estado = CServicioExtra.EliminarServicio(servicioExtra.IdServicioExtra);
                MessageBox.Show("Servicio eliminado");
                ListarSvE();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
