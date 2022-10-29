using Controlador;
using Modelo;
using System.Data;

namespace Pruebas
{
    public class DepartamentoTest
    {
        [Fact]
        //Agregar departamento
        public void TestAgregarDepto()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Comuna comuna = new()
            {
                IdComuna = 1
            };
            Departamento departamento = new()
            {
                TarifaDiara = 30000,
                Direccion = "Avenida San Benito",
                NroDpto = 608,
                Capacidad = 4,
                Comuna = comuna
            };

            //Act
            resObtenido = Controlador.CDepartamento.CrearDepto(departamento);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Actualizar departamento
        public void TestActualizarDepto()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Comuna comuna = new()
            {
                IdComuna = 1
            };
            Departamento departamento = new()
            {
                IdDepto = 2, 
                TarifaDiara = 25000,
                Direccion = "Avenida San Pablo",
                NroDpto = 607,
                Capacidad = 5,
                Comuna = comuna
            };

            //Act
            resObtenido = Controlador.CDepartamento.ActualizarDepto(departamento);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Listar departamento
        public void TestListarDepto()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            DataTable departamento;

            //Act
            departamento = CDepartamento.ListarDpto();
            resObtenido = departamento.Rows.Count;
            
            //Assert
            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Eliminar departamento
        public void TestEliminarDepto()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;

            //Act   se usa el ID a eliminar
            resObtenido = Controlador.CDepartamento.EliminarDpto(2);

            Assert.Equal(resEsperado, resObtenido);
        }
    }
}