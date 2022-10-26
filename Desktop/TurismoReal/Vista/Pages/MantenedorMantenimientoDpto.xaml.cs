using Controlador;
using Modelo;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vista.Pages
{
    public partial class MantenedorMantenimientoDpto : Page
    {
        private Departamento departamento;
        public MantenedorMantenimientoDpto(Departamento depto)
        {
            InitializeComponent();
            departamento = depto;
            ListarMantencion();
        }
        private void ItemError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }

        private void btnAbrirAgregarMantDpto_Click(object sender, RoutedEventArgs e)
        {
            dhMantDpto_ag.IsOpen = true;
        }

        private void DtgMantDptosUpdate_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void DtgMantDptoDelete_Click(object sender, RoutedEventArgs e)
        {
            Mantencion mantencion = (Mantencion)dtgMantDptos.SelectedItem;
            try
            {
                MessageBoxResult result = MessageBox.Show("Estás seguro de querer eliminar esta mantención?", "Mantenimiento", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int estado = CMantenimientoDpto.EliminarMantDpto(mantencion.IdMantencion);
                    MensajeOk("Departamento eliminado");
                    ListarMantencion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Mantención", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MensajeOk(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Mantención", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ListarMantencion()
        {
            try
            {
                DataTable dataTable = CMantenimientoDpto.ListarMantenimiento(departamento.IdDepto);
                if (dataTable != null)
                {
                    var mantenciones = (from rw in dataTable.AsEnumerable()
                                 select new Mantencion()
                                 {
                                     IdMantencion = Convert.ToInt32(rw[0]),
                                     NombreMantenimiento = rw[2].ToString(),
                                     DescripcionMantenimiento = rw[3].ToString(),
                                     FechaInicio = DateTime.Parse(rw[4].ToString()),
                                     FechaTermino = DateTime.Parse(rw[5].ToString()),
                                     CostoMantencion = Convert.ToInt32(rw[7]),
                                     Estado = rw[6].ToString()                                    
                                 }).ToList();
                    dtgMantDptos.ItemsSource = mantenciones;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_Agregar_MantDpto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_nombre_ag.Text == string.Empty || txt_desc_ag.Text == string.Empty || dp_inicio_ag.Text == string.Empty ||
                    dp_termino_ag.Text == string.Empty || departamento == null)
                {
                    this.MensajeError("Falta ingresar algunos datos");
                }
                else
                {
                    Mantencion mant = new Mantencion
                    {
                        NombreMantenimiento = txt_nombre_ag.Text.Trim(),
                        DescripcionMantenimiento = txt_desc_ag.Text.Trim(),
                        FechaInicio = Convert.ToDateTime(dp_inicio_ag.SelectedDate.Value.Date.ToString("dd-MM-yyyy")),
                        FechaTermino = Convert.ToDateTime(dp_termino_ag.SelectedDate.Value.Date.ToString("dd-MM-yyyy")),
                        Estado = "p",
                        CostoMantencion = Convert.ToInt32(txt_costo_ag.Text.Trim())
                    };

                    int estado = CMantenimientoDpto.CrearMantDepto(mant, departamento.IdDepto);
                    MensajeOk("Mantención agregada");
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.StackTrace);
            }
        }
        private void Limpiar()
        {
            txt_nombre_ag.Clear();
            txt_desc_ag.Clear();
            txt_costo_ag.Clear();
            dp_inicio_ag.SelectedDate = null;
            dp_termino_ag.SelectedDate = null;
        }

        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhMantDpto_ag.IsOpen = false;
        }
    }
}
