using Controlador;
using Modelo;
using System.Data;

namespace Pruebas
{
    public class InventarioTest
    {
        //[Fact]
        ////Agregar inventario
        public void TestCrearInventario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Objeto objeto = new()
            {
                IdObjeto = 1
            };
            Inventario inventario = new()
            {
                ValorTotal = 150000
            };

            //Act   se coloca ID Depto al final
            resObtenido = Controlador.CInventario.CrearInventario(objeto, 1);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Actualizar inventario
        public void TestActualizarInventario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Objeto objeto = new()
            {
                IdObjeto = 3
            };
            Inventario inventario = new()
            {
                ValorTotal = 150000
            };

            //Act   se coloca ID Depto al final
            resObtenido = Controlador.CInventario.ActualizarInventario(objeto);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Listar inventario
        public void TestListarInventario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            DataTable inventario;

            //Act   se coloca ID de departamento
            inventario = CInventario.ListarInventario(1);
            resObtenido = inventario.Rows.Count;

            //Assert
            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Eliminar inventario
        public void TestEliminarInventario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Objeto objeto = new();
            Inventario inventario = new();

            //Act   se usa el ID a eliminar
            resObtenido = Controlador.CInventario.EliminarObjeto(1);

            Assert.Equal(resEsperado, resObtenido);
        }
    }
}