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
using System.Windows.Shapes;
using Vista.PagesFuncionario;

namespace Vista
{
    /// <summary>
    /// Interaction logic for MenuFuncionario.xaml
    /// </summary>
    public partial class MenuFuncionario : Window
    {
        private readonly DataTable dt;
        public MenuFuncionario(DataTable funcionario)
        {
            InitializeComponent();
            dt = funcionario;
            Default();
        }
        private void Default()
        {
            MainMenu mainMenu = new(dt);
            PagesNavigation.Navigate(mainMenu);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mainMenu = new(dt);
            PagesNavigation.Navigate(mainMenu);
        }

        private void btnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new Uri("PagesFuncionario/CheckIn.xaml", UriKind.RelativeOrAbsolute));

        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
