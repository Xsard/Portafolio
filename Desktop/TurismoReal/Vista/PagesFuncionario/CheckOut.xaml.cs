﻿using Controlador;
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
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Page
    {
        public CheckOut()
        {
            InitializeComponent();
            ListarReservas();
        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reserva reserva = (Reserva)dtgReservas.SelectedItem;
                int IdReserva = reserva.IdReserva;
                char FirmaFunc = '1';
                char EstadoR = 'T';
                char EstadoP = 'L';
                MessageBoxResult result = MessageBox.Show("¿Desea ingresar multas?", "Reservas", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                }
                else
                {
                    CReserva.ConfirmarFirma(IdReserva, FirmaFunc, EstadoR, EstadoP);
                    MensajeOk("Check out realizado con éxito");
                    ListarReservas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void MensajeOk(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Reservas", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ListarReservas()
        {
            try
            {
                DataTable dataTable = CReserva.ListarReservas(1);
                if (dataTable != null)
                {
                    foreach (var row in dataTable.AsEnumerable())
                    {
                        if (row[3].ToString() == "E")
                            row[3] = "Iniciada";
                        
                    }
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
    }
}
