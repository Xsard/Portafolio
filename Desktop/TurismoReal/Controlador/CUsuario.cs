using Oracle.ManagedDataAccess.Client;
using System.Data;

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
        public static int ContarUsuarios()
        {
            int resultado = new();
            using (OracleConnection con = Conexion.getInstance().ConexionDB())
            {
                OracleCommand cmd = new()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "contar_Usuarios"
                };
                cmd.Parameters.Add("R", OracleDbType.Int32, ParameterDirection.Output);
                try
                {
                    con.Open();
                    cmd.ExecuteReader();
                    resultado = int.Parse(cmd.Parameters["r"].Value.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                    cmd.Dispose();
                }
            }
            return resultado;
        }
    }
}
