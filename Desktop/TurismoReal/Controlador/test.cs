using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Controlador
{
    public class test : IOracleCustomType, INullable
    {
        private bool ObjectIsNull;

        [OracleObjectMapping("nombre")]
        public string Nombre { get; set; }

        [OracleObjectMapping("rol")]
        public int Rol { get; set; }

        public bool IsNull { get { return ObjectIsNull; } }

        public void FromCustomObject(OracleConnection con, object udt)
        {
            OracleUdt.SetValue(con, udt, "nombre", Nombre);
            OracleUdt.SetValue(con, udt, "rol", Rol);
        }

        public void ToCustomObject(OracleConnection con, object udt)
        {
            Nombre = (string)OracleUdt.GetValue(con, udt, "nombre");
            Rol = (int)OracleUdt.GetValue(con, udt, "rol");
        }
        public static test Null
        {
            get
            {
                test obj = new test();
                obj.ObjectIsNull = true;
                return obj;
            }
        }
    }
    [OracleCustomTypeMapping("c##turismo_real.usuario_con")]
    public class testaFactory : IOracleCustomTypeFactory
    {
        public IOracleCustomType CreateObject()
        {
            return new test();
        }
    }

}
