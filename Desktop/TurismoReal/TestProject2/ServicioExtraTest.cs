using Controlador;
using Modelo;
using System.Data;

namespace Pruebas
{
    public class ServicioExtraTest
    {
        [Fact]
        //Insertar servicio 
        public void TestIngresarServicio()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            ServicioExtra servicioExtra = new()
            {
                NombreServicioExtra = "Desayuno",
                DescripcionServicioExtra = "Servicio de desayuno",
                ValorServicioExtra = 10000
            };

            //Act
            resObtenido = Controlador.CServicioExtra.IngresarServicio(servicioExtra);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Actualizar servicio
        public void TestActualizarServicio()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            ServicioExtra servicioExtra = new()
            {
                NombreServicioExtra = "Desayuno premium",
                DescripcionServicioExtra = "Servicio de desayuno premium",
                ValorServicioExtra = 15000
            };

            //Act
            resObtenido = Controlador.CServicioExtra.ActualizarServicio(servicioExtra);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Listar servicio
        public void TestListarDServicio()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            DataTable servicioExtra;

            //Act
            servicioExtra = CServicioExtra.ListarServicios();
            resObtenido = servicioExtra.Rows.Count;

            //Assert
            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Eliminar servicio
        public void TestEliminarServicio()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            ServicioExtra servicioExtra = new();

            //Act   se usa el ID a eliminar
            resObtenido = Controlador.CServicioExtra.EliminarServicio(67);

            Assert.Equal(resEsperado, resObtenido);
        }
    }
}
