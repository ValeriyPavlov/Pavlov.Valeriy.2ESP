using Microsoft.VisualStudio.TestTools.UnitTesting;
using BibliotecaClases;
using System.Collections.Generic;
using System;

namespace Test_UsuarioDao
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_TraerUsuarios_1()
        {
            //Arrange: Inicializar variables.
            string comando = "SELECT * FROM dbo.usuarios WHERE usuario = 'admin'";
            //Act: Llamar al metodo a testear.
            List<Usuario> usuarios = UsuarioDao.TraerUsuarios(comando);
            //Assert: comprobar el valor con lo esperado.
            Assert.AreEqual(1, usuarios.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_TraerUsuarios_2()
        {
            //Arrange: Inicializar variables.
            string comando = "";
            //Act: Llamar al metodo a testear.
            List<Usuario> usuarios = UsuarioDao.TraerUsuarios(comando);
        }

        [TestMethod]
        public void Test_TraerUsuarios_3()
        {
            //Arrange: Inicializar variables.
            string comando = "SELECT * FROM dbo.usuarios WHERE usuario = 'noExiste'";
            //Act: Llamar al metodo a testear.
            List<Usuario> usuarios = UsuarioDao.TraerUsuarios(comando);
            //Assert: comprobar el valor con lo esperado.
            Assert.AreEqual(0, usuarios.Count);
        }

    }
}
