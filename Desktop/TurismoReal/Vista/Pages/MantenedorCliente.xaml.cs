﻿using Controlador;
using Modelo;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vista.Pages.Validaciones;

namespace Vista.Pages
{
    public partial class MantenedorCliente : Page
    {
        public MantenedorCliente()
        {
            InitializeComponent();
            ListarCliente();
        }
        private void ItemError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }
        private void ListarCliente()
        {
            try
            {
                DataTable dataTable = CCliente.ListarCliente();
                if (dataTable != null)
                {
                    var cliente = (from rw in dataTable.AsEnumerable()
                                   select new Cliente()
                                   {
                                       IdUsuario = Convert.ToInt32(rw[0]),
                                       Rut = rw[8].ToString(),
                                       Nombres = rw[9].ToString(),
                                       Apellidos = rw[10].ToString(),
                                       Email = rw[1].ToString(),
                                       Telefono = Convert.ToInt32(rw[3])
                                   }).ToList();
                    dtgCliente.ItemsSource = cliente;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_Agregar_Cliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_rut_ag.Text == string.Empty || txt_nombres_ag.Text == string.Empty || txt_apellidos_ag.Text == string.Empty ||
                    txt_fono_ag.Text == string.Empty || txt_email_ag.Text == string.Empty || txt_pass_ag.Password == string.Empty || txt_passConfirm_ag.Password == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos");
                }
                else
                {
                    string pattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
                    if (!Regex.IsMatch(txt_pass_ag.Password, pattern) || txt_pass_ag.Password != txt_passConfirm_ag.Password)
                    {
                        MessageBox.Show("Las contraseñas no coinciden o no son lo suficientemente seguras");
                        return;
                    }
                    string nRut = txt_rut_ag.Text.Split('-').First();
                    string dvRut = txt_rut_ag.Text.Split('-').Last();
                    if (!Rut.ValidaRut(nRut, dvRut))
                    {
                        MessageBox.Show("Rut invalido");
                        return;
                    }
                    pattern = "^\\S+@\\S+\\.\\S+$";
                    if (!Regex.IsMatch(txt_email_ag.Text, pattern))
                    {
                        MessageBox.Show("Correo inválido");
                        return;
                    }
                    Cliente userCliente = new()
                    {
                        Email = txt_email_ag.Text.Trim(),
                        Contraseña = txt_pass_ag.Password.Trim(),
                        Telefono = Convert.ToInt32(txt_fono_ag.Text.Trim()),
                        Rut = txt_rut_ag.Text.Trim(),
                        Nombres = txt_nombres_ag.Text.Trim(),
                        Apellidos = txt_apellidos_ag.Text.Trim(),
                    };

                    int estado = CCliente.CrearUsuarioCliente(userCliente);
                    MensajeOk("Cliente agregado");
                    ListarCliente();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.StackTrace);
            }
        }
        private void DtgClienteUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Cliente userCliente = (Cliente)dtgCliente.SelectedItem;
                    try
                    {
                        int estado = CCliente.ActualizarCliente(userCliente);
                        MensajeOk("Cliente actualizado");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.StackTrace);
            }
        }
        private void DtgClienteDelete_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = (Cliente)dtgCliente.SelectedItem;
            try
            {
                MessageBoxResult result = MessageBox.Show("Estás seguro de querer eliminar este usuario cliente?", "Clientes", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int estado = CAdmin.EliminarAdmin(cliente.IdUsuario);
                    MensajeOk("Cliente eliminado");
                    ListarCliente();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Limpiar()
        {
            txt_email_ag.Clear();
            txt_pass_ag.Clear();
            txt_passConfirm_ag.Clear();
            txt_fono_ag.Clear();
            txt_rut_ag.Clear();
            txt_nombres_ag.Clear();
            txt_apellidos_ag.Clear();
        }
        private void btnAbrirAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            dhCliente_ag.IsOpen = true;
        }
        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhCliente_ag.IsOpen = false;
        }
        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Clientes", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MensajeOk(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Clientes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void txt_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zá-úÁ-Ú0-9\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void txt_fono_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void txt_rut_ag_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_rut_ag.Text.Length >= 2)
            {
                txt_rut_ag.Text = txt_rut_ag.Text.ToString().Insert(txt_rut_ag.Text.Length - 1, "-");
            }
        }

        private void txt_rut_ag_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_rut_ag.Text = txt_rut_ag.Text.Replace("-", "");
        }

        private void txt_rut_ag_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9k\"]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
