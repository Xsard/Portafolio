using Controlador;
using System.Data;
using System.Windows;

namespace Vista
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Ingresar_button_Click(object sender, RoutedEventArgs e)
        {
            string email = "desktop@gmail.com"; //email_txt.Text;
            string psw = "123"; //pass_txt.Password.ToString();
            DataTable dt = CUsuario.Autentificar(email, psw);
            if (dt.Rows.Count != 0)
            {
                if (dt.Rows[0][1].Equals("Administrador"))
                {
                    MenuAdmin menuAdmin = new(dt);
                    menuAdmin.Show();
                    this.Close();
                }
                else if(dt.Rows[0][1].Equals("Funcionario"))
                {
                    MessageBox.Show("Bienvenid@ Funcionari@ "+ dt.Rows[0][0].ToString());
                }
            }
            else
            {
                MessageBox.Show("Email o contraseña no válidos");
            }
        }
    }
}
