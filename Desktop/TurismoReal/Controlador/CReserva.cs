using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Controlador
{
    public class CReserva
    {
        public static DataTable ListarReservas()
        {
            DataTable resultado = new();
            using (OracleConnection con = Conexion.getInstance().ConexionDB())
            {
                OracleCommand cmd = new()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Mantener_Reserva.listar_reserva"
                };
                cmd.Parameters.Add("Reservas", OracleDbType.RefCursor, ParameterDirection.Output);
                try
                {
                    con.Open();
                    cmd.ExecuteReader();
                    OracleDataAdapter da = new()
                    {
                        SelectCommand = cmd
                    };
                    da.Fill(resultado);
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

        public static int ConfirmarCheckIn(int idReserva, DateTime CheckIn)
        {
            int resultado;
            using (OracleConnection con = Conexion.getInstance().ConexionDB())
            {
                OracleCommand cmd = new()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Mantener_Reserva.actualizar_checkin"
                };
                cmd.Parameters.Add("identificador", OracleDbType.Int32, ParameterDirection.Input).Value = idReserva;
                cmd.Parameters.Add("checkin", OracleDbType.Date, ParameterDirection.Input).Value = CheckIn;
                cmd.Parameters.Add("r", OracleDbType.Int32, ParameterDirection.Output);
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
