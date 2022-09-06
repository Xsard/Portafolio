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
using System.Windows.Shapes;

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        public MenuAdmin()
        {
            InitializeComponent();
            ListarDpto();
        }

        #region Departamento 
        private void ListarDpto()
        {
            List<Comuna> comunas = CComuna.ListarComuna();
            (Resources["comunas"] as CollectionViewSource).Source = comunas;
            DataTable dataTable = CDepartamento.ListarDpto();
            var Dptos = (from rw in dataTable.AsEnumerable()
                         select new Departamento()
                         {
                             IdDepto = Convert.ToInt32(rw[0]),
                             TarifaDiara = Convert.ToInt32(rw[1]),
                             Direccion = rw[2].ToString(),
                             NroDpto = Convert.ToInt32(rw[3]),
                             Capacidad = Convert.ToInt32(rw[4]),
                             Comuna = new Comuna { IdComuna = Convert.ToInt32(rw[5]), NombreComuna = rw[7].ToString() }
                         }).ToList();
            dtgDptos.ItemsSource = Dptos;
        }
        private void btn_Dpto_Crud_Click(object sender, RoutedEventArgs e)
        {
            grdMainMenu.Visibility = Visibility.Collapsed;
            grdAgregar.Visibility = Visibility.Visible;

        }
        private void btnAbrirAgregarDpto_Click(object sender, RoutedEventArgs e)
        {
            dhDpto_ag.IsOpen = true;
        }

        private void btn_Agregar_Dpto_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txt_tarifa_ag.Text, out int tarifa))
            {
                if (int.TryParse(txt_nro_ag.Text, out int nro))
                {
                    if (int.TryParse(txt_cap_ag.Text, out int capacidad))
                    {
                        string direccion = txt_direccion_ag.Text;
                        Comuna comuna = new Comuna
                        {
                            IdComuna = 2
                        };
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
        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhDpto_ag.IsOpen = false;
        }

        #endregion

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Departamento departamento = (Departamento)dtgDptos.SelectedItem;
                MessageBox.Show(departamento.Direccion);
            }
        }

    }
}
