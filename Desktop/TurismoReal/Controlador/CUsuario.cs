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
    public class CUsuario
    {

        public DataTable Autientificar(string email, string psw)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection con = Conexion.getInstance().ConexionDB())
            {
                OracleCommand cmd = new ();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "login_desk.AUTENTIFICAR";
                cmd.Parameters.Add("usr_con", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
                cmd.Parameters.Add("email_aut", OracleDbType.Varchar2, ParameterDirection.Input).Value = email;
                cmd.Parameters.Add("psw_aut", OracleDbType.Varchar2, ParameterDirection.Input).Value = psw;
                
                con.Open();
                cmd.ExecuteReader();
                da.SelectCommand = cmd;
                da.Fill(dataTable);
                con.Close();
            }
            return dataTable;
        }
    }
}
