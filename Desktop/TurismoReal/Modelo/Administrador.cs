namespace Modelo
{
    public class Administrador
    {
        private int idUsuario;
        private string rut;
        private string nombres;
        private string apellidos;        
        private string email;
        private string contraseña;
        private int telefono;

        public string Rut { get => rut; set => rut = value; }
        public string Nombres { get => nombres; set => nombres = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public string Email { get => email; set => email = value; }
        public int Telefono { get => telefono; set => telefono = value; }
        public string Contraseña { get => contraseña; set => contraseña = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    }
}