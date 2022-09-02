using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Oracle.ManagedDataAccess.Client;

namespace Controlador
{
    public class CUsuario
    {
        public DataSet Autientificar(string email, string psw)
        {
            DataSet ds = new();
            OracleDataAdapter da = new();
            OracleCommand cmd = new();
            OracleConnection? con = Conexion.getInstance().ConexionDB();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AUTENTIFICAR";
                cmd.Parameters.Add("email_aut", OracleDbType.Varchar2, 254).Value = email;
                cmd.Parameters.Add("psw_aut", OracleDbType.Varchar2, 30).Value = psw;
                cmd.Connection = con;

                con.Open();
                cmd.ExecuteReader();
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
        public DataSet Prueba()
        {
            DataSet ds = new();
            OracleDataAdapter da = new();
            OracleCommand cmd = new();
            OracleConnection? con = Conexion.getInstance().ConexionDB();
            try
            {
                cmd.CommandText = "Select * from USUARIO";
                cmd.Connection = con;

                con.Open();
                cmd.ExecuteReader();
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
    }
}
