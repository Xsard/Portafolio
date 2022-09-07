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
using Controlador;

namespace Vista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Ingresar_button_Click(object sender, RoutedEventArgs e)
        {
            MenuAdmin menuAdmin = new();
            menuAdmin.Show();
            this.Close();

            string email = email_txt.Text;
            string psw = pass_txt.Password.ToString();            
            DataTable dt = CUsuario.Autientificar(email, psw);
            if (dt.Rows.Count != 0)
            {
                if (dt.Rows[0][1].Equals("Administrador"))
                {

                }
                else if(dt.Rows[0][1].Equals("Funcionario"))
                {
                    MessageBox.Show("Bienvenid@ Funcionari@ "+ dt.Rows[0][0].ToString());
                }
            }
        }
    }
}
