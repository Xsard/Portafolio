using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Mantencion
    {
        private int idMantencion;
        private Departamento departamento;
        private string nombreMantenimiento;
        private string descripcionMantenimiento;
        private DateTime fechaInicio;
        private DateTime fechaTermino;
        private string estado;
        private int costoMantencion;

        public int IdMantencion { get => idMantencion; set => idMantencion = value; }
        public Departamento Departamento { get => departamento; set => departamento = value; }
        public string NombreMantenimiento { get => nombreMantenimiento; set => nombreMantenimiento = value; }
        public string DescripcionMantenimiento { get => descripcionMantenimiento; set => descripcionMantenimiento = value; }
        public DateTime FechaInicio { get => fechaInicio; set => fechaInicio = value; }
        public DateTime FechaTermino { get => fechaTermino; set => fechaTermino = value; }
        public string Estado { get => estado; set => estado = value; }
        public int CostoMantencion { get => costoMantencion; set => costoMantencion = value; }
    }
}
