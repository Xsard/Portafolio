using Controlador;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vista.Pages
{
    public partial class MantenedorMantenimientoDpto : Page
    {
        private Departamento departamento;
        private List<DateTime> fechasInicio = new();
        private List<DateTime> fechasTermino = new();
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
            if (e.Key == Key.Enter)
            {
                Mantencion mantencion = (Mantencion)dtgMantDptos.SelectedItem;
                try
                {
                    int estado = CMantenimientoDpto.ActualizarMantDepto(mantencion);
                    MessageBox.Show("Mantención actualizada");
                    ListarMantencion();
                }
                catch (Exception)
                {
                    throw;
                }
            }
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
                    MensajeOk("Mantención eliminada");
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
                                     IdDepto = departamento.IdDepto,
                                     NombreMantenimiento = rw[1].ToString(),
                                     DescripcionMantenimiento = rw[2].ToString(),
                                     FechaInicio = DateTime.Parse(rw[3].ToString()),
                                     FechaTermino = DateTime.Parse(rw[4].ToString()),
                                     CostoMantencion = Convert.ToInt32(rw[5]),
                                     Estado = rw[6].ToString()                                    
                                 }).ToList();
                    foreach (Mantencion item in mantenciones)
                    {
                        fechasInicio.Add(item.FechaInicio);
                        fechasTermino.Add(item.FechaTermino);
                        dp_inicio_ag.BlackoutDates.Add(new CalendarDateRange(item.FechaInicio, item.FechaTermino));
                        dp_termino_ag.BlackoutDates.Add(new CalendarDateRange(item.FechaInicio, item.FechaTermino));
                    }
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
                    ListarMantencion();
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

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker datePickerGrd = (DatePicker)sender;
            var test = dp_inicio_ag.BlackoutDates.ToArray();

            foreach (var item in test)
            {
                DateTime fechaPervia = default(DateTime);
                DateTime fechaSiguiente = default(DateTime);
                if (datePickerGrd.SelectedDate == item.End)
                {
                    fechasInicio.Sort((a, b) => a.CompareTo(b));
                    fechaSiguiente = fechasInicio.SkipWhile(x => x <= datePickerGrd.SelectedDate).FirstOrDefault();
                    if (!fechaSiguiente.Equals(default(DateTime)))
                    {
                        datePickerGrd.DisplayDateEnd = fechaSiguiente.AddDays(-1);
                    }
                }
                else if (datePickerGrd.SelectedDate == item.Start)
                {
                    fechasInicio.Sort((a, b) => a.CompareTo(b));
                    fechaPervia = fechasTermino.SkipWhile(x => x >= datePickerGrd.SelectedDate).FirstOrDefault();
                    if (!fechaPervia.Equals(default(DateTime)))
                    {
                        datePickerGrd.DisplayDateStart = fechaPervia.AddDays(1);
                    }
                }
            }

        }

        private void DatePickerFechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePickerGrd = (DatePicker)sender;
            Mantencion mantencion = (Mantencion)dtgMantDptos.SelectedItem;
            if (mantencion == null) return;
            if (datePickerGrd.SelectedDate >= mantencion.FechaTermino)
            {
                MessageBox.Show("La fecha seleccionada no puede ser mayor o igual a la de término");
                return;
            }
        }

        private void DatePickerFechaTer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePickerGrd = (DatePicker)sender;
            Mantencion mantencion = (Mantencion)dtgMantDptos.SelectedItem;
            if (mantencion == null) return;
            if (datePickerGrd.SelectedDate <= mantencion.FechaInicio)
            {
                MessageBox.Show("La fecha seleccionada no puede ser menor o igual a la de inicio");
                return;
            }
        }

        private void txt_string_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú0-9\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_costo_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
