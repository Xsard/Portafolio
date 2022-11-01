using Controlador;
using Modelo;
using System.Data;
using System;
using System.Windows.Controls;
using System.Linq;

namespace Vista.Pages
{
    public partial class PlanificarTransporte : Page
    {
        public PlanificarTransporte()
        {
            InitializeComponent();
            ListarReservas();
        }

        private void ListarReservas()
        {
            try
            {
                DataTable dataTable = CReserva.ListarReservas();
                if (dataTable != null)
                {
                    var Trans = (from rw in dataTable.AsEnumerable()
                                select new Reserva()
                                {
                                    IdReserva = Convert.ToInt32(rw[0]),
                                    IdDepto = Convert.ToInt32(rw[1]),
                                    IdCliente = Convert.ToInt32(rw[2]),
                                    EstadoReserva = rw[3].ToString(),
                                    EstadoPago = rw[4].ToString(),
                                    CheckIn = DateTime.Parse(rw[5].ToString()),
                                    CheckOut = DateTime.Parse(rw[6].ToString()),
                                    Firma = rw[7].ToString(),
                                    CantidadAcompanantes = Convert.ToInt32(rw[8]),
                                    Transporte = rw[9].ToString(),
                                    ValorTotal = Convert.ToInt32(rw[10]),
                                }).ToList();
                    dtgTransporte.ItemsSource = Trans;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btn_planificar_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
