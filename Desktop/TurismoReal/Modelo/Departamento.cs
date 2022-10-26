namespace Modelo
{
    public class Departamento
    {
        private int idDepto;
        private int tarifaDiara;
        private string direccion;
        private int nroDpto;
        private int capacidad;
        private Comuna comuna;
        private string nombreDpto;

        public int IdDepto { get => idDepto; set => idDepto = value; }
        public int TarifaDiara { get => tarifaDiara; set => tarifaDiara = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public int NroDpto { get => nroDpto; set => nroDpto = value; }
        public int Capacidad { get => capacidad; set => capacidad = value; }
        public Comuna Comuna { get => comuna; set => comuna = value; }
        public string NombreDpto { get => nombreDpto; set => nombreDpto = value; }
    }
}
