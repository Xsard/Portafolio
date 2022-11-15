using QuestPDF.Fluent;
using QuestPDF.Helpers;
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
        }

        private void Btn_Reportes_Click(object sender, RoutedEventArgs e)
        {
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
                            column.Spacing(25);
                            foreach (var item in Enumerable.Range(0,10))
                            {
                                column.Item()
                                      .Background(Colors.Grey.Medium)
                                      .Height(100)
                                      .Text("UN 1 POR PAYASOS");
                            }
                        });
                    page.Footer()
                    .Background(Colors.Green.Lighten2);
                });

            }).GeneratePdf(filePath);

            Process.Start("explorer.exe", filePath);
        }
    }
}
