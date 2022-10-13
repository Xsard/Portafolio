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

namespace Vista.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu(DataTable dt)
        {
            InitializeComponent();
            lblAdmin.Content = dt.Rows[0][0].ToString();
        }
        private void btn_Dpto_Crud_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new System.Uri("Pages/MantenedorDpto.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btn_ServE_Crud_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new System.Uri("Pages/MantenedorServExtras.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btn_Usuarios_Crud_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new System.Uri("Pages/MantenedorUsuario.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btn_Disponibilidad_Crud_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new System.Uri("Pages/MantenedorDisponibilidad.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
