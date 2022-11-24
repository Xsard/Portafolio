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
            var model = InvoiceDocumentDataSource.GetInvoiceDetails();
            var document = new InvoiceDocument(model);

            GenerateDocumentAndShow(document);
        }

        private void GenerateDocumentAndShow(InvoiceDocument document)
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

    public class InvoiceDocument : IDocument
    {
        public InvoiceModel Model { get; }

        public InvoiceDocument(InvoiceModel model)
        {
            Model = model;
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
                        .Item().Text($"Invoice #{Model.InvoiceNumber}")
                        .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    Column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span($"{Model.IssueDate:d}");
                    });

                    Column.Item().Text(text =>
                    {
                        text.Span("Due date: ").SemiBold();
                        text.Span($"{Model.DueDate:d}");
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

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
                    row.ConstantItem(50);
                    row.RelativeItem().Component(new AddressComponent("For", Model.CustomerAddress));
                });

                column.Item().Element(ComposeTable);

                var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
                column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {totalPrice}$").SemiBold();

                if (!string.IsNullOrWhiteSpace(Model.Comments))
                    column.Item().PaddingTop(25).Element(ComposeComments);
            });
        }

        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold();

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Product").Style(headerStyle);
                    header.Cell().AlignRight().Text("Unit price").Style(headerStyle);
                    header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                    header.Cell().AlignRight().Text("Total").Style(headerStyle);

                    header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                });

                foreach (var item in Model.Items)
                {
                    table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}$");
                    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity}$");

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            });
        }

        void ComposeComments(IContainer container)
        {
            container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Comments").FontSize(14).SemiBold();
                column.Item().Text(Model.Comments);
            });
        }
    }

    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private Address Address { get; }

        public AddressComponent(string title, Address address)
        {
            Title = title;
            Address = address;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                column.Item().Text(Title).SemiBold();
                column.Item().PaddingBottom(5).LineHorizontal(1);

                column.Item().Text(Address.CompanyName);
                column.Item().Text(Address.Street);
                column.Item().Text($"{Address.City}, {Address.State}");
                column.Item().Text(Address.Email);
                column.Item().Text(Address.Phone);
            });
        }
    }

    public static class InvoiceDocumentDataSource
    {
        private static Random Random = new Random();

        public static InvoiceModel GetInvoiceDetails()
        {
            var items = Enumerable
                .Range(1, 25)
                .Select(i => GenerateRandomOrderItem())
                .ToList();

            return new InvoiceModel
            {
                InvoiceNumber = Random.Next(1_000, 10_000),
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now + TimeSpan.FromDays(14),

                SellerAddress = GenerateRandomAddress(),
                CustomerAddress = GenerateRandomAddress(),

                Items = items,
                Comments = Placeholders.Paragraph()
            };
        }

        private static OrderItem GenerateRandomOrderItem()
        {
            return new OrderItem
            {
                Name = Placeholders.Label(),
                Price = (decimal)Math.Round(Random.NextDouble() * 100, 2),
                Quantity = Random.Next(1, 10)
            };
        }

        private static Address GenerateRandomAddress()
        {
            return new Address
            {
                CompanyName = Placeholders.Name(),
                Street = Placeholders.Label(),
                City = Placeholders.Label(),
                State = Placeholders.Label(),
                Email = Placeholders.Email(),
                Phone = Placeholders.PhoneNumber()
            };
        }
    }

    public class InvoiceModel
    {
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public Address SellerAddress { get; set; }
        public Address CustomerAddress { get; set; }

        public List<OrderItem> Items { get; set; }
        public string Comments { get; set; }
    }

    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Address
    {
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public object Email { get; set; }
        public string Phone { get; set; }
    }

}
