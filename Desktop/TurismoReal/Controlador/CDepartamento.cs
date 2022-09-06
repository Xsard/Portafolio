﻿using Modelo;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public static class CDepartamento
    {
        public static int CrearDepto(Departamento dpto)
        {
            int resultado = 0;
            using (OracleConnection con = Conexion.getInstance().ConexionDB())
            {
                OracleCommand cmd = new()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Mantener_Dpto.insertar_dpto"
                };
                cmd.Parameters.Add("r", OracleDbType.Int32, ParameterDirection.ReturnValue);
                cmd.Parameters.Add("tarifa", OracleDbType.Int32, ParameterDirection.Input).Value = dpto.TarifaDiara;
                cmd.Parameters.Add("DIREC", OracleDbType.Varchar2, ParameterDirection.Input).Value = dpto.Direccion;
                cmd.Parameters.Add("NRO", OracleDbType.Int32, ParameterDirection.Input).Value = dpto.NroDpto;
                cmd.Parameters.Add("CAP", OracleDbType.Int32, ParameterDirection.Input).Value = dpto.Capacidad;
                cmd.Parameters.Add("COMUNA", OracleDbType.Int32, ParameterDirection.Input).Value = dpto.Comuna.IdComuna;

                try
                {
                    con.Open();
                    cmd.ExecuteReader();
                    resultado = int.Parse(cmd.Parameters["r"].Value.ToString());
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }

            }
            return resultado;
        }
        public static DataTable ListarDpto()
        {
            DataTable resultado = new();
            using (OracleConnection con = Conexion.getInstance().ConexionDB())
            {
                OracleCommand cmd = new()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Mantener_Dpto.listar_dpto"
                };
                cmd.Parameters.Add("Deptos", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
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
                }
            }
            return resultado;
        }
    }
}