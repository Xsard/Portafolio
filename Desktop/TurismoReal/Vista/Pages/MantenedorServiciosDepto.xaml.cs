using Controlador;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
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
    /// Interaction logic for MantenedorServiciosDepto.xaml
    /// </summary>
    public partial class MantenedorServiciosDepto : Page
    {
        Departamento departamento;
        int col = 0;
        int colC = 0;

        public MantenedorServiciosDepto(Departamento dpto)
        {
            InitializeComponent();
            departamento = dpto;
            ListarServicios();
            ListarServiciosContratados();
        }
        

        public void ListarServicios()
        {
            try
            {
                DataTable dataTable = CServDpto.ListarServiciosDpto(departamento.IdDepto);
                if (dataTable != null)
                {
                    var ServDpto = (from rw in dataTable.AsEnumerable()
                                    select new Servicio()
                                    {
                                        IdServDpto = Convert.ToInt32(rw[0]),
                                        NombreServDpto = rw[1].ToString(),
                                        DescServDpto = rw[2].ToString(),
                                    }).ToList();
                    foreach (var item in ServDpto)
                    {
                        GenerarElementos(item);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ListarServiciosContratados()
        {
            try
            {
                DataTable dataTable = CServDpto.ListarServiciosAsignadosDpto(departamento.IdDepto);
                if (dataTable != null)
                {

                    var ServDpto = (from rw in dataTable.AsEnumerable()
                                    select new ServDpto()
                                    {
                                        IdServDpto = Convert.ToInt32(rw[0]),
                                        IdDpto = Convert.ToInt32(rw[1]),
                                        Estado = Convert.ToInt32(rw[2]),
                                        NombreServDpto = rw[3].ToString(),
                                        DescServDpto = rw[4].ToString(),
                                    }).ToList();
                    foreach (var item in ServDpto)
                    {
                        GenerarElementos(item);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void GenerarElementos(Servicio item)
        {
            Label lblTitulo = new()
            {
                Content = item.NombreServDpto,
                Height = 30,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Name = "lblT" + item.IdServDpto
            };

            TextBox lblDesc = new()
            {
                Text = item.DescServDpto,
                Height = 60,
                Margin = new Thickness(2, 30, 2, 0),
                VerticalAlignment = VerticalAlignment.Top,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                BorderThickness = new Thickness(0),
                Focusable = false,
                Name = "lblD" + item.IdServDpto
            };

            Button btnAgregar = new()
            {
                Margin = new Thickness(0, 100, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Agregar",
                Height = 30,
                Width = 100,
                Name = "btn" + item.IdServDpto
            };

            Grid contenedor = new()
            {
                Height = 135,
                Name = "grd" + item.IdServDpto.ToString()
            };

            contenedor.Children.Add(lblTitulo);
            contenedor.Children.Add(lblDesc);
            contenedor.Children.Add(btnAgregar);

            Border borde = new()
            {
                Name = "brd" + item.IdServDpto,
                Margin = new Thickness(2),
                CornerRadius = new CornerRadius(3),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = contenedor
            };


            if (col == 0)
            {
                stkServ1.Children.Add(borde);
                stkServ1.RegisterName(lblTitulo.Name, lblTitulo);
                stkServ1.RegisterName(lblDesc.Name, lblDesc);
                stkServ1.RegisterName(contenedor.Name, contenedor);
                btnAgregar.Click += delegate (object sender, RoutedEventArgs e) { BtnAgregarServicioDpto(sender, e, borde, 0, item); };
                col = 1;
            }
            else
            {
                stkServ2.Children.Add(borde);
                stkServ2.RegisterName(lblTitulo.Name, lblTitulo);
                stkServ2.RegisterName(lblDesc.Name, lblDesc);
                stkServ2.RegisterName(contenedor.Name, contenedor);
                btnAgregar.Click += delegate (object sender, RoutedEventArgs e) { BtnAgregarServicioDpto(sender, e, borde, 1, item); };
                col = 0;
            }
        }

        private void GenerarElementos(ServDpto item)
        {
            Label lblTitulo = new()
            {
                Content = item.NombreServDpto,
                Height = 30,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Name = "lblTC" + item.IdServDpto
            };
            Label auxEstado = new() { Content = item.Estado, Visibility = Visibility.Hidden, Name = "aux" + item.IdServDpto};
            TextBox lblDesc = new()
            {
                Text = item.DescServDpto,
                Height = 60,
                Margin = new Thickness(2, 30, 2, 0),
                VerticalAlignment = VerticalAlignment.Top,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                BorderThickness = new Thickness(0),
                Focusable = false,
                Name = "lblDC" + item.IdServDpto
            };

            Button btnQuitar= new()
            {
                Margin = new Thickness(20, 100, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Quitar",
                Height = 30,
                Width = 100,
                Name = "btnQ" + item.IdServDpto
            };
            Button btnAcEstado = new()
            {
                Margin = new Thickness(0, 100, 20, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Height = 30,
                Width = 100,
                Name = "btnAg" + item.IdServDpto
            };

            if (item.Estado == 0)
            {
                btnAcEstado.Content = "Activar";
            }
            else
            {
                btnAcEstado.Content = "Desctivar";

            }

            Grid contenedor = new()
            {
                Height = 135,
                Name = "grdC" + item.IdServDpto.ToString()
            };

            contenedor.Children.Add(lblTitulo);
            contenedor.Children.Add(lblDesc);
            contenedor.Children.Add(btnQuitar);
            contenedor.Children.Add(btnAcEstado);
            contenedor.Children.Add(auxEstado);

            Border borde = new()
            {
                Name = "brdC" + item.IdServDpto,
                Margin = new Thickness(2),
                CornerRadius = new CornerRadius(3),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = contenedor
            };


            if (colC == 0)
            {
                stkServDpto1.Children.Add(borde);
                stkServDpto1.RegisterName(lblTitulo.Name, lblTitulo);
                stkServDpto1.RegisterName(lblDesc.Name, lblDesc);
                stkServDpto1.RegisterName(contenedor.Name, contenedor);
                btnAcEstado.Click += delegate (object sender, RoutedEventArgs e) { BtnActServicioDpto(sender, e, item.IdServDpto, auxEstado, btnAcEstado); };
                colC = 1;
            }
            else
            {
                stkServDpto2.Children.Add(borde);
                stkServDpto2.RegisterName(lblTitulo.Name, lblTitulo);
                stkServDpto2.RegisterName(lblDesc.Name, lblDesc);
                stkServDpto2.RegisterName(contenedor.Name, contenedor);
                btnAcEstado.Click += delegate (object sender, RoutedEventArgs e) { BtnActServicioDpto(sender, e, item.IdServDpto, auxEstado, btnAcEstado); };
                colC = 0;
            }
        }

        #region
        private void BtnAgregarServicioDpto(object sender, RoutedEventArgs e, Border border, int posStk, Servicio serv)
        {
            int resultado = CServDpto.IngresarServicioDpto(serv.IdServDpto, departamento.IdDepto, 0);

            if (resultado == -21201) return;

            if (posStk == 0) stkServ1.Children.Remove(border);            
            if (posStk == 1) stkServ2.Children.Remove(border);
            ServDpto servDpto = new() { IdServDpto = serv.IdServDpto, NombreServDpto = serv.NombreServDpto, DescServDpto = serv.DescServDpto, Estado = 0, IdDpto = departamento.IdDepto };
            GenerarElementos(servDpto);
        }
        private void BtnActServicioDpto(object sender, RoutedEventArgs e, int id, Label estado, Button button)
        {
            int resultado;
            if ((int)estado.Content == 0)
            {
                resultado = CServDpto.ActualizarServicioDpto(id, departamento.IdDepto, 1);
                if (resultado == -21202) return;
                estado.Content = 1;
                button.Content = "Desctivar";
            }
            else
            {
                resultado = CServDpto.ActualizarServicioDpto(id, departamento.IdDepto, 0);
                if (resultado == -21202) return;
                estado.Content = 0;
                button.Content = "Activar";
            }
        }
        #endregion
    }
}
