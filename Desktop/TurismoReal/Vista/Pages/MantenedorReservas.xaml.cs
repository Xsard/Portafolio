﻿using Controlador;
using System.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using Modelo;
using System.Linq;

namespace Vista.Pages
{
    public partial class MantenedorReservas : Page
    {
        public MantenedorReservas()
        {
            InitializeComponent();
            ListarReservas();
        }
        private void ItemError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }

        private void btn_planificar_Click(object sender, RoutedEventArgs e)
        {
            dhCorreo.IsOpen = true;
        }

        private void btn_Enviar_Correo_Click(object sender, RoutedEventArgs e)
        {
            Reserva reserva = (Reserva)dtgReservas.SelectedItem;
            if (reserva != null)
            {
                string email = txt_nombre_ag.Text;
                string asunto = txtAsunto.Text;
                string lugar = txtTerminal.Text;
                string comuna = txtComuna.Text;
                if (email == null || asunto == null || lugar == null || comuna == null)
                {
                    return;
                }
                Mensajeria.PlanificarTransporte(email, asunto, reserva.CantidadAcompanantes.ToString(), lugar, comuna, reserva.CheckIn.ToString(), reserva.CheckOut.ToString(), reserva.Dpto.NombreDpto, reserva.Dpto.Direccion);
                dhCorreo.IsOpen = false;
            }
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            dhCorreo.IsOpen = false;
        }
        private void ListarReservas()
        {
            try
            {
                DataTable dataTable = CReserva.ListarReservas();
                if (dataTable != null)
                {
                    var reservas = (from rw in dataTable.AsEnumerable()
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
                                     Cliente = new Cliente { Rut = rw[11].ToString(), Nombres = rw[12].ToString(), Apellidos = rw[13].ToString() },
                                     Dpto = new Departamento { NombreDpto = rw[15].ToString(), Direccion = rw[17].ToString() }
                                 }).ToList();
                    dtgReservas.ItemsSource = reservas;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btn_ConfirmarCheckin_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Estás seguro de actualizar la fecha de checkin?", "Reservas", MessageBoxButton.YesNo, MessageBoxImage.Question);
            Reserva reserva = (Reserva)dtgReservas.SelectedItem;
            int idReserva = reserva.IdReserva;
            DateTime checkin = DateTime.Now;
            CReserva.ConfirmarCheckIn(idReserva, checkin);
            MessageBox.Show("CheckIn Actualizado");
            ListarReservas();
        }

        private void btn_ConfirmarCheckOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
