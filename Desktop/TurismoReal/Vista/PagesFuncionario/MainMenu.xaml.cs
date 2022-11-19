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

namespace Vista.PagesFuncionario
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu(DataTable dt)
        {
            InitializeComponent();
            lblFunc.Content = dt.Rows[0][0].ToString();
        }

        private void btn_Checkout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_CheckIn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
