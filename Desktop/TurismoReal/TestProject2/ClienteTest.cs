using Controlador;
using Modelo;
using System.Data;

namespace Pruebas
{
    public class ClienteTest
    {
        [Fact]
        //Agregar cliente
        public void TestAgregarCliente()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Cliente cliente = new()
            {
                Rut = "19837402-7",
                Nombres = "Emilia Antonia",
                Apellidos = "Fernandez Acuña",
                Email = "emfernandez@gmail.com",
                Contraseña = "emilia1234",
                Telefono = 982146592
            };

            //Act
            resObtenido = Controlador.CCliente.CrearUsuarioCliente(cliente);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Actualizar cliente
        public void TestActualizarCliente()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Cliente cliente = new()
            {
                Rut = "19837402-7",
                Nombres = "Emilia Sofia",
                Apellidos = "Fernandez Acuña",
                Email = "emfernandez@gmail.com",
                Contraseña = "emilia1234",
                Telefono = 982146592
            };

            //Act
            resObtenido = Controlador.CCliente.ActualizarCliente(cliente);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Listar cliente
        public void TestListarCliente()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            DataTable cliente;

            //Act
            cliente = CCliente.ListarCliente();
            resObtenido = cliente.Rows.Count;

            //Assert
            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Eliminar cliente
        public void TestEliminarCliente()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Cliente cliente = new();

            //Act   se usa el ID a eliminar
            resObtenido = Controlador.CCliente.EliminarCliente(01);

            Assert.Equal(resEsperado, resObtenido);
        }
    }
}