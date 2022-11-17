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
                DataTable dataTable = CServicio.ListarServiciosDpto();
                if (dataTable != null)
                {
                    var ServDpto = (from rw in dataTable.AsEnumerable()
                                    select new Servicio()
                                    {
                                        IdServDpto = Convert.ToInt32(rw[0]),
                                        NombreServDpto = rw[1].ToString(),
                                        DescServDpto = rw[2].ToString(),
                                    }).ToList();
                    int col = 0;
                    foreach (var item in ServDpto)
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
                            Margin = new Thickness(2,30,2,0),
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
                            VerticalAlignment= VerticalAlignment.Top,
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
                        

                        if (col== 0)
                        {
                            stkServ1.Children.Add(borde);
                            stkServ1.RegisterName(lblTitulo.Name, lblTitulo);
                            stkServ1.RegisterName(lblDesc.Name, lblDesc);
                            stkServ1.RegisterName(contenedor.Name, contenedor);
                            btnAgregar.Click += delegate (object sender, RoutedEventArgs e) { BtnAgregarServicioDpto(sender, e, item.IdServDpto, borde, 0); };
                            col = 1;
                        }
                        else
                        {
                            stkServ2.Children.Add(borde);
                            stkServ2.RegisterName(lblTitulo.Name, lblTitulo);
                            stkServ2.RegisterName(lblDesc.Name, lblDesc);
                            stkServ2.RegisterName(contenedor.Name, contenedor);
                            btnAgregar.Click += delegate (object sender, RoutedEventArgs e) { BtnAgregarServicioDpto(sender, e, item.IdServDpto, borde, 1); };
                            col = 0;
                        }
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
                DataTable dataTable = CServDpto.ListarServiciosDpto(departamento.IdDepto);
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
                    int col = 0;
                    MessageBox.Show(dataTable.ToString());

                    foreach (var item in ServDpto)
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

                        Button btnAgregar = new()
                        {
                            Margin = new Thickness(0, 100, 0, 0),
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            Content = "Agregar",
                            Height = 30,
                            Width = 100,
                            Name = "btnC" + item.IdServDpto
                        };

                        Grid contenedor = new()
                        {
                            Height = 135,
                            Name = "grdC" + item.IdServDpto.ToString()
                        };

                        contenedor.Children.Add(lblTitulo);
                        contenedor.Children.Add(lblDesc);
                        contenedor.Children.Add(btnAgregar);

                        Border borde = new()
                        {
                            Name = "brdC" + item.IdServDpto,
                            Margin = new Thickness(2),
                            CornerRadius = new CornerRadius(3),
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(1),
                            Child = contenedor
                        };


                        if (col == 0)
                        {
                            stkServDpto1.Children.Add(borde);
                            stkServDpto1.RegisterName(lblTitulo.Name, lblTitulo);
                            stkServDpto1.RegisterName(lblDesc.Name, lblDesc);
                            stkServDpto1.RegisterName(contenedor.Name, contenedor);
                            btnAgregar.Click += delegate (object sender, RoutedEventArgs e) { BtnAgregarServicioDpto(sender, e, item.IdServDpto, borde, 0); };
                            col = 1;
                        }
                        else
                        {
                            stkServDpto2.Children.Add(borde);
                            stkServDpto2.RegisterName(lblTitulo.Name, lblTitulo);
                            stkServDpto2.RegisterName(lblDesc.Name, lblDesc);
                            stkServDpto2.RegisterName(contenedor.Name, contenedor);
                            btnAgregar.Click += delegate (object sender, RoutedEventArgs e) { BtnAgregarServicioDpto(sender, e, item.IdServDpto, borde, 1); };
                            col = 0;
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region
        private void BtnAgregarServicioDpto(object sender, RoutedEventArgs e, int id, Border border, int posStk)
        {
            int resultado = CServDpto.IngresarServicioDpto(id, departamento.IdDepto, 0);

            if (resultado == -21201) return;

            if (posStk == 0) stkServ1.Children.Remove(border);            
            if (posStk == 1) stkServ2.Children.Remove(border);
        }
        #endregion
    }
}
