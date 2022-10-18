using Modelo;

namespace Pruebas
{
    public class DepartamentoTest
    {
        [Fact]
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
    }
}