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

namespace Vista.PagesFuncionario
{
    /// <summary>
    /// Interaction logic for CheckList.xaml
    /// </summary>
    public partial class CheckList : Page
    {
        Reserva reserva;
        public CheckList(Reserva res)
        {
            InitializeComponent();
            reserva = res;
            ListarObjetos();
        }
        private void ListarObjetos()
        {
            try
            {
                DataTable dataTable = CInventario.ListarInventario(reserva.IdDepto);
                if (dataTable != null)
                {
                    var Objetos = (from rw in dataTable.AsEnumerable()
                                   select new Objeto()
                                   {
                                       IdObjeto = Convert.ToInt32(rw[0]),
                                       NombreObjeto = rw[2].ToString(),
                                       CantidadObjeto = Convert.ToInt32(rw[3]),
                                       ValorUnitarioObjeto = Convert.ToInt32(rw[4])
                                   }).ToList();
                    foreach (var item in Objetos)
                    {
                        List<int> lista = new();
                        for (int i = 0; i <= item.CantidadObjeto; i++) lista.Add(i);
                        item.AuxCant = lista;
                        dtgItems.Items.Add(item);
                        MessageBox.Show(dtgItems.Items.CurrentPosition.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
