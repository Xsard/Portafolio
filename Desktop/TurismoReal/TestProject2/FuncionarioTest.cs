using Controlador;
using Modelo;
using System.Data;

namespace Pruebas
{
    public class FuncionarioTest
    {
        [Fact]
        //Agregar funcionario
        public void TestAgregarFuncionario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Funcionario funcionario = new()
            {
                Rut = "17836278-3",
                Nombres = "Felipe Esteban",
                Apellidos = "Diaz Jara",
                Email = "fdiazjara@gmail.com",
                Contraseña = "felipe1234",
                Telefono = 973287367
            };

            //Act
            resObtenido = Controlador.CFuncionario.CrearUsuarioFuncionario(funcionario);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Actualizar funcionario
        public void TestActualizarFuncionario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Funcionario funcionario = new()
            {
                Rut = "17836278-3",
                Nombres = "Felipe Simón",
                Apellidos = "Diaz Jara",
                Email = "fdiazjara@gmail.com",
                Contraseña = "felipe1234",
                Telefono = 973287367
            };

            //Act
            resObtenido = Controlador.CFuncionario.ActualizarFuncionario(funcionario);

            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Listar funcionario
        public void TestListarFuncionario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            DataTable funcionario;

            //Act
            funcionario = CFuncionario.ListarFuncionario();
            resObtenido = funcionario.Rows.Count;

            //Assert
            Assert.Equal(resEsperado, resObtenido);
        }

        [Fact]
        //Eliminar funcionario
        public void TestEliminarFuncionario()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Funcionario funcionario = new();

            //Act   se usa el ID a eliminar
            resObtenido = Controlador.CFuncionario.EliminarFuncionario(67);

            Assert.Equal(resEsperado, resObtenido);
        }
    }
}
