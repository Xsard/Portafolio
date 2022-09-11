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
    /// Interaction logic for Mantenedor_Dpto.xaml
    /// </summary>
    public partial class MantenedorDpto : Page
    {
        public MantenedorDpto()
        {
            InitializeComponent();
            ListarComunas();
            ListarDpto();
        }
        #region Ubicacion
        public void ListarComunas()
        {
            try
            {
                List<Comuna> comunas = CComuna.ListarComuna();
                if (comunas != null)
                {
                    (Resources["comunas"] as CollectionViewSource).Source = comunas;
                    cbo_comuna_ag.ItemsSource = comunas;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region Departamento 
        private void ListarDpto()
        {
            try
            {
                DataTable dataTable = CDepartamento.ListarDpto();
                if (dataTable != null)
                {
                    var Dptos = (from rw in dataTable.AsEnumerable()
                                 select new Departamento()
                                 {
                                     IdDepto = Convert.ToInt32(rw[0]),
                                     TarifaDiara = Convert.ToInt32(rw[1]),
                                     Direccion = rw[2].ToString(),
                                     NroDpto = Convert.ToInt32(rw[3]),
                                     Capacidad = Convert.ToInt32(rw[4]),
                                     Comuna = new Comuna
                                     {
                                         IdComuna = Convert.ToInt32(rw[5]),
                                         NombreComuna = rw[7].ToString()
                                     }
                                 }).ToList();
                    dtgDptos.ItemsSource = Dptos;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btnAbrirAgregarDpto_Click(object sender, RoutedEventArgs e)
        {
            dhDpto_ag.IsOpen = true;
        }
        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhDpto_ag.IsOpen = false;
        }
        private void btn_Agregar_Dpto_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txt_tarifa_ag.Text, out int tarifa))
            {
                if (int.TryParse(txt_nro_ag.Text, out int nro))
                {
                    if (int.TryParse(txt_cap_ag.Text, out int capacidad))
                    {
                        if (cbo_comuna_ag.SelectedItem != null)
                        {
                            string direccion = txt_direccion_ag.Text;
                            Comuna comuna = (Comuna)cbo_comuna_ag.SelectedItem;
                            Departamento dpto = new Departamento
                            {
                                TarifaDiara = tarifa,
                                Capacidad = capacidad,
                                Direccion = direccion,
                                NroDpto = nro,
                                Comuna = comuna
                            };
                            int estado = CDepartamento.CrearDepto(dpto);
                            MessageBox.Show(estado.ToString());
                        }

                    }
                }
            }
        }
        private void DtgDptosUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Departamento departamento = (Departamento)dtgDptos.SelectedItem;
                try
                {
                    int estado = CDepartamento.ActualizarDepto(departamento);
                    MessageBox.Show(estado.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private void DtgDptosUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Departamento departamento = (Departamento)dtgDptos.SelectedItem;
            if (departamento != null)
            {
                try
                {
                    int estado = CDepartamento.ActualizarDepto(departamento);
                    MessageBox.Show(estado.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private void DtgDptoDelete_Click(object sender, RoutedEventArgs e)
        {
            Departamento departamento = (Departamento)dtgDptos.SelectedItem;
            try
            {
                int estado = CDepartamento.EliminarDpto(departamento.IdDepto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void DtgDptoDetalles_Click(object sender, RoutedEventArgs e)
        {
            Departamento departamento = (Departamento)dtgDptos.SelectedItem;
            NavigationService ns = NavigationService.GetNavigationService(this);
            PerfilDepto perfilDepto = new(departamento);
            ns.Navigate(perfilDepto);
        }
        #endregion
    }
}
