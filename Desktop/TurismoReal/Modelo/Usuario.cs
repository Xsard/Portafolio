namespace Modelo
{
    public class Usuario
    {
        private int idUsuario;
        private string email;
        private string contraseña;

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Email { get => email; set => email = value; }
        public string Contraseña { get => contraseña; set => contraseña = value; }
    }
}