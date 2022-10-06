using Controlador;
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
using System.Windows.Shapes;
using Vista.Pages;

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        private DataTable dt;
        public MenuAdmin(DataTable admin)
        {
            InitializeComponent();
            dt = admin;
            Default();
        }
        #region Barra de navegación
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
        private void btnDpto_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Pages/MantenedorDpto.xaml", UriKind.RelativeOrAbsolute));

        }
        private void btnServE_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Pages/MantenedorServExtras.xaml", UriKind.RelativeOrAbsolute));
        }
        private void btnUsuario_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Pages/MantenedorUsuario.xaml", UriKind.RelativeOrAbsolute));
        }
        #endregion
    }
}
