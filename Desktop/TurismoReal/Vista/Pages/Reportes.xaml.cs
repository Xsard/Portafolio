using Controlador;
using Modelo;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Vista.Pages
{
    public partial class Reportes : Page
    {
        public Reportes()
        {
            InitializeComponent();
            ListarRegiones();
            ListarComunas();
            ListarDpto();
        }
        private void ItemError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }
        public void ListarComunas()
        {
            try
            {
                List<Comuna> comunas = CComuna.ListarComuna();
                if (comunas != null)
                {
                    cbo_Comunas.ItemsSource = comunas;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ListarRegiones()
        {
            try
            {
                List<Region> regiones = CRegion.ListarRegion();
                if (regiones != null)
                {
                    cbo_Regiones.ItemsSource = regiones;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ListarDpto()
        {
            try
            {
                DataTable dataTable = CDepartamento.ListarDpto();
                if (dataTable != null)
                {
                    var Dptos = (from rw in dataTable.AsEnumerable()
                                 select new Departamento()
                                 {
                                     IdDepto = Convert.ToInt32(rw[0]),
                                     NombreDpto = rw[1].ToString(),
                                     TarifaDiara = Convert.ToInt32(rw[2]),
                                     Direccion = rw[3].ToString(),
                                     NroDpto = Convert.ToInt32(rw[4]),
                                     Capacidad = Convert.ToInt32(rw[5]),
                                     Comuna = new Comuna
                                     {
                                         IdComuna = Convert.ToInt32(rw[6]),
                                         NombreComuna = rw[9].ToString()
                                     },
                                     Disponibilidad = Convert.ToBoolean(rw[7])
                                 }).ToList();
                    cbo_Dptos.ItemsSource = Dptos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnGenReporteStats_Click(object sender, RoutedEventArgs e)
        {
            DataTable deptos = CDepartamento.ListarDpto();
            //var entries = new[]
            //{
            //    new ChartEntry(212)
            //    {
            //        Label = "Cantidad departamentos",
            //        ValueLabel = deptos.AsEnumerable().Count().ToString(),
            //        Color = SKColor.Parse("#2c3e50")
            //    }
            //};

            //var filePath = "TESTreporte.pdf";
            //Document.Create(document =>
            //{
            //    document.Page(page =>
            //    {
            //        page.Header()
            //            .Background(Colors.Blue.Lighten2);
            //        page.Content()
            //            .Column(column =>
            //            {
            //                var titleStyle = TextStyle
            //                    .Default
            //                    .FontSize(20)
            //                    .SemiBold()
            //                    .FontColor(Colors.Blue.Medium);

            //                column
            //                    .Item()
            //                    .PaddingBottom(10)
            //                    .Text("Chart example")
            //                    .Style(titleStyle);

            //                column
            //                    .Item()
            //                    .Border(1)
            //                    .ExtendHorizontal()
            //                    .Height(300)
            //                    .Canvas((canvas, size) =>
            //                    {
            //                        var chart = new BarChart
            //                        {
            //                            Entries = entries,

            //                            LabelOrientation = Orientation.Horizontal,
            //                            ValueLabelOrientation = Orientation.Horizontal,

            //                            IsAnimated = false,
            //                        };

            //                        chart.DrawContent(canvas, (int)size.Width, (int)size.Height);
            //                    });
            //            });
            //.Column(column =>
            //{
            //    column.Spacing(25);
            //    foreach (var item in Enumerable.Range(0, 10))
            //    {
            //        column.Item()
            //              .Background(Colors.Grey.Medium)
            //              .Height(100)
            //              .Text("UN 1 POR PAYASOS");
            //    }
            //});
            //page.Footer()
            //        .Background(Colors.Green.Lighten2);
            //    });

            //}).GeneratePdf(filePath);

            //Process.Start("explorer.exe", filePath);
        }
        private void btnGenReporteReservas_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            int nivel = 0;
            
            if (cbo_Dptos.SelectedIndex > 0)
            {
                Departamento departamento = (Departamento)cbo_Dptos.SelectedItem;
                MessageBox.Show(departamento.NombreDpto);
                cbo_Dptos.SelectedIndex = -1;
            }
            //var model = InvoiceDocumentDataSource.GetInvoiceDetails();
            //var document = new ReporteDocumento(model);

            //GenerateDocumentAndShow(document);
        }

        private void GenerateDocumentAndShow(ReporteDocumento document)
        {
            const string filePath = "invoice.pdf";

            document.GeneratePdf(filePath);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo(filePath)
                {
                    UseShellExecute = true
                }
            };

            process.Start();
        }
    }

    public class ReporteDocumento : IDocument
    {
        public List<ReporteReserva> Modelo { get; }

        public ReporteDocumento(List<ReporteReserva> modelo)
        {
            Modelo = modelo;
        }        

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(Column =>
                {
                    Column
                        .Item().Text($"Reporte #1")
                        .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    Column.Item().Text(text =>
                    {
                        text.Span("Fecha del reporte: ").SemiBold();
                        text.Span($"{DateTime.Now:d}");
                    });
                });

                row.ConstantItem(100).Height(50).Placeholder();
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(20);

                //column.Item().Row(row =>
                //{
                //    row.RelativeItem().Component(new AddressComponent("From", Modelo.SellerAddress));
                //    row.ConstantItem(50);
                //    row.RelativeItem().Component(new AddressComponent("For", Modelo.CustomerAddress));
                //});

                column.Item().Element(ComposeTable);

                //var totalPrice = Modelo.Items.Sum(x => x.Price * x.Quantity);
                //column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {totalPrice}$").SemiBold();

                //if (!string.IsNullOrWhiteSpace(Modelo.Comments))
                //    column.Item().PaddingTop(25).Element(ComposeComments);
            });
        }
        
        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold();

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(250);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    //header.Cell().Text("#");
                    header.Cell().Text("Nombre Departamento").Style(headerStyle);
                    header.Cell().AlignRight().Text("Cant. Arriendos").Style(headerStyle);
                    header.Cell().AlignRight().Text("Duración prom. de reservas").Style(headerStyle);
                    header.Cell().AlignRight().Text("Cant. Multas").Style(headerStyle);

                    header.Cell().ColumnSpan(4).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                });



            foreach (var item in Modelo)
            {
                    //table.Cell().Element(CellStyle).Text(Modelo.Items.IndexOf(item) + 1);
                    table.Cell().Element(CellStyle).Text(item.NombreDpto);
                    table.Cell().Element(CellStyle).AlignRight().Text(item.CantArriendos);
                    table.Cell().Element(CellStyle).AlignRight().Text(item.PromDiasReserva);
                    table.Cell().Element(CellStyle).AlignRight().Text(item.CantMultas);

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            });
        }

        void ComposeComments(IContainer container)
        {
            container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(5);
                //column.Item().Text("Comments").FontSize(14).SemiBold();
                //column.Item().Text(Modelo.Comments);
            });
        }
    }

    //public class AddressComponent : IComponent
    //{
    //    private string Title { get; }
    //    private Address Address { get; }

    //    public AddressComponent(string title, Address address)
    //    {
    //        Title = title;
    //        Address = address;
    //    }

    //    public void Compose(IContainer container)
    //    {
    //        container.ShowEntire().Column(column =>
    //        {
    //            column.Spacing(2);

    //            column.Item().Text(Title).SemiBold();
    //            column.Item().PaddingBottom(5).LineHorizontal(1);

    //            //column.Item().Text(Address.CompanyName);
    //            //column.Item().Text(Address.Street);
    //            //column.Item().Text($"{Address.City}, {Address.State}");
    //            //column.Item().Text(Address.Email);
    //            //column.Item().Text(Address.Phone);
    //        });
    //    }
    //}

    public static class InvoiceDocumentDataSource
    {

        public static List<ReporteReserva> GetInvoiceDetails(int id, int nivel, DateTime fecha_inicio, DateTime fecha_termino)
        {          
            DataTable dt = CReporte.GenReporteReserva(id, nivel, fecha_inicio, fecha_termino);
            var Dptos = (from rw in dt.AsEnumerable()
                         select new ReporteReserva()
                         {
                             FechaReporte = DateTime.Now,
                             NombreDpto = (string)rw[0],
                             CantArriendos = (decimal)rw[1],
                             PromDiasReserva = (decimal)rw[2],
                             CantMultas = (decimal)rw[3],
                             Comments = Placeholders.Paragraph()
                         }).ToList();

            return Dptos;
        }

        //private static void GenerarReporteReserva()
        //{
        //    DataTable dt = CReporte.GenReporteReserva();
        //    var Dptos = (from rw in dt.AsEnumerable()
        //                 select new ReporteReserva()
        //                 {
        //                     FechaReporte = DateTime.Now,
        //                     NombreDpto = (string)rw[0],
        //                     CantArriendos = (int)rw[1],
        //                     PromDiasReserva = (int)rw[2],
        //                     CantMultas = (int)rw[3],
        //                     Comments = Placeholders.Paragraph()
        //                 }).ToList();
        //}

        //private static OrderItem GenerateRandomOrderItem()
        //{
        //    return new OrderItem
        //    {
        //        Name = Placeholders.Label(),
        //        Price = (decimal)Math.Round(Random.NextDouble() * 100, 2),
        //        Quantity = Random.Next(1, 10)
        //    };
        //}

        //private static Address GenerateRandomAddress()
        //{
        //    return new Address
        //    {
        //        CompanyName = Placeholders.Name(),
        //        Street = Placeholders.Label(),
        //        City = Placeholders.Label(),
        //        State = Placeholders.Label(),
        //        Email = Placeholders.Email(),
        //        Phone = Placeholders.PhoneNumber()
        //    };
        //}
    }
}
