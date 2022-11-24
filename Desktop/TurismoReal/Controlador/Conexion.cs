using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Controlador
{
    class Conexion
    {
        private string connectionString = "User Id=turismo_real; Password=TurismoReal22; Data Source=(description= (retry_count=20)(retry_delay=3)(address=(protocol=tcps)(port=1521)(host=adb.sa-santiago-1.oraclecloud.com))(connect_data=(service_name=gca56ca291beb8f_turismoreal_high.adb.oraclecloud.com))(security=(ssl_server_dn_match=yes)))";
        private static Conexion Con = null;

        public OracleConnection ConexionDB()
        {
            OracleConnection con = new();
            try
            {
                con.ConnectionString = connectionString;
            }
            catch (Exception ex)
            {
                throw;
            }
            return con;
        }
        public static Conexion getInstance()
        {
            if (Con == null)
            {
                Con = new Conexion();
            }
            return Con;
        }
    }
}
