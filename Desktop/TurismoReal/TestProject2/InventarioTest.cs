using Modelo;

namespace Pruebas
{
    public class InventarioTest
    {
        //[Fact]
        ////Agregar inventario
        //public void TestCrearInventario()
        //{
        //    //Arrange
        //    int resEsperado = 1;
        //    int resObtenido;
        //    Objeto objeto = new()
        //    {
        //        IdObjeto = 1
        //    };
        //    Inventario inventario = new()
        //    {
        //        ValorTotal = 150000
        //    };

        //    //Act
        //    resObtenido = Controlador.CInventario.CrearInventario(inventario,1);

        //    Assert.Equal(resEsperado, resObtenido);
        //}

        [Fact]
        //Actualizar inventario
        public void TestActualizarInventario()
        {
            ////Arrange
            //int resEsperado = 1;
            //int resObtenido;
            //Comuna comuna = new()
            //{
            //    IdComuna = 1
            //};
            //Departamento departamento = new()
            //{
            //    IdDepto = 65,
            //    TarifaDiara = 3000,
            //    Direccion = "Avenida la tula",
            //    NroDpto = 6,
            //    Capacidad = 1,
            //    Comuna = comuna
            //};

            ////Act
            //resObtenido = Controlador.CDepartamento.ActualizarDepto(departamento);

            //Assert.Equal(resEsperado, resObtenido);
        }

        //[Fact]
        //Listar inventario


        [Fact]
        //Eliminar inventario
        public void TestEliminarInventario()
        {
            ////Arrange
            //int resEsperado = 1;
            //int resObtenido;
            //Departamento departamento = new();

            ////Act   se usa el ID a eliminar
            //resObtenido = Controlador.CDepartamento.EliminarDpto(67);

            //Assert.Equal(resEsperado, resObtenido);
        }
    }
}