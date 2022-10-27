using Controlador;
using Modelo;
using System.Data;

namespace Pruebas
{
    public class ComunaTest
    {
        [Fact]
        //Listar comuna
        public void TestListarComuna()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            List<Comuna> comuna;

            //Act
            comuna = CComuna.ListarComuna();
            resObtenido = comuna.Count;

            //Assert
            Assert.Equal(resEsperado, resObtenido);
        }
    }
}
