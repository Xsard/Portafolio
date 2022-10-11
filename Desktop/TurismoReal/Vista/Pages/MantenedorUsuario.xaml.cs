using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vista.Pages
{
    public partial class MantenedorUsuario : Page
    {
        public MantenedorUsuario()
        {
            InitializeComponent();
        }

        private void DtgUsuariosUpdate_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnAbrirAgregarUsuario_Click(object sender, RoutedEventArgs e)
        {
            dhUsuario_ag.IsOpen = true;
        }
        private void btn_Cancelar_Ag_Click(object sender, RoutedEventArgs e)
        {
            dhUsuario_ag.IsOpen = false;
        }
        private void Limpiar()
        {
            txt_rut_ag.Clear();
            txt_nombres_ag.Clear();
            txt_apellidos_ag.Clear();
            txt_email_ag.Clear();
            txt_password_ag.Clear();
            txt_rol_ag.Clear();
        }
        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Usuarios", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MensajeOk(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Usuarios", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DtgUsuariosDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Agregar_Usuario_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
