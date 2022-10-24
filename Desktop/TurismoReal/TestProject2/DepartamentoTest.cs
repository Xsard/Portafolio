using Modelo;

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
                Direccion = "Avenida las tulas",
                NroDpto = 69,
                Capacidad = 11,
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
                IdDepto = 65, 
                TarifaDiara = 3000,
                Direccion = "Avenida la tula",
                NroDpto = 6,
                Capacidad = 1,
                Comuna = comuna
            };

            //Act
            resObtenido = Controlador.CDepartamento.ActualizarDepto(departamento);

            Assert.Equal(resEsperado, resObtenido);
        }

        //[Fact]
        //Listar departamento
        

        [Fact]
        //Eliminar departamento
        public void TestEliminarDepto()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Departamento departamento = new();

            //Act   se usa el ID a eliminar
            resObtenido = Controlador.CDepartamento.EliminarDpto(67);

            Assert.Equal(resEsperado, resObtenido);
        }
    }
}