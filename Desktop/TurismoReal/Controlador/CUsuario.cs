using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Controlador
{
    public static class CUsuario
    {
        public static DataTable Autentificar(string email, string psw)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection con = Conexion.getInstance().ConexionDB())
            {
                OracleCommand cmd = new()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "login_desk.AUTENTIFICAR"
                };
                cmd.Parameters.Add("email_aut", OracleDbType.Varchar2, ParameterDirection.Input).Value = email;
                cmd.Parameters.Add("psw_aut", OracleDbType.Varchar2, ParameterDirection.Input).Value = psw;
                cmd.Parameters.Add("usr_con", OracleDbType.RefCursor, ParameterDirection.Output);

                try
                {
                    con.Open();
                    cmd.ExecuteReader();
                    OracleDataAdapter da = new()
                    {
                        SelectCommand = cmd
                    };
                    da.Fill(dataTable);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }

            }
            return dataTable;
        }
    }
}
