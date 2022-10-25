﻿using Modelo;

namespace Pruebas
{
    public class FotografiaTest
    {
        [Fact]
        //Insertar imagen
        public void TestInsertarImagen()
        {
            //Arrange
            int resEsperado = 1;
            int resObtenido;
            Departamento departamento = new()
            {
                IdDepto = 1
            };
            Fotografia fotografia = new()
            {
                Path_img = "volcan.jpg",
                Alt = "Volcán Villarica"
            };

            //Act
            resObtenido = Controlador.CFotografia.InsertarImagen(fotografia);

            Assert.Equal(resEsperado, resObtenido);
        }

        //[Fact]
        //Listar imágenes
    }
}