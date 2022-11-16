using Controlador;
using Microcharts;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Orientation = Microcharts.Orientation;

namespace Vista.Pages
{
    public partial class Reportes : Page
    {
        public Reportes()
        {
            InitializeComponent();
        }

        private void Btn_Reportes_Click(object sender, RoutedEventArgs e)
        {
            DataTable deptos = CDepartamento.ListarDpto();

            var entries = new[]
            {
                new ChartEntry(212)
                {
                    Label = "Cantidad departamentos",
                    ValueLabel = deptos.AsEnumerable().Count().ToString(),
                    Color = SKColor.Parse("#2c3e50")
                }
            };

            var filePath = "TESTreporte.pdf";
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Header()
                        .Background(Colors.Blue.Lighten2);
                    page.Content()
                        .Column(column =>
                        {
                            var titleStyle = TextStyle
                                .Default
                                .FontSize(20)
                                .SemiBold()
                                .FontColor(Colors.Blue.Medium);

                            column
                                .Item()
                                .PaddingBottom(10)
                                .Text("Chart example")
                                .Style(titleStyle);

                            column
                                .Item()
                                .Border(1)
                                .ExtendHorizontal()
                                .Height(300)
                                .Canvas((canvas, size) =>
                                {
                                    var chart = new BarChart
                                    {
                                        Entries = entries,

                                        LabelOrientation = Orientation.Horizontal,
                                        ValueLabelOrientation = Orientation.Horizontal,

                                        IsAnimated = false,
                                    };

                                    chart.DrawContent(canvas, (int)size.Width, (int)size.Height);
                                });
                        });
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
                    page.Footer()
                    .Background(Colors.Green.Lighten2);
                });

            }).GeneratePdf(filePath);

            Process.Start("explorer.exe", filePath);
        }
    }
}
