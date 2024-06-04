using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;




namespace Persistencia
{
    internal class PersitenciaVuelo :IPeristenciaVuelo
    {

        //singleton 
        private static PersitenciaVuelo _instancia = null;
        private PersitenciaVuelo() { }
        public static PersitenciaVuelo GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersitenciaVuelo();
            return _instancia;
        }

        // operaciones 
        public void AltaVuelo(Vuelo V,Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
                        SqlCommand _comando = new SqlCommand("AltaVuelo", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@IDVuelo", V.IDvuelo);
            _comando.Parameters.AddWithValue("@CantidadAsiento", V.CantidadAsientos);
            _comando.Parameters.AddWithValue("@PrecioVuelo", V.PrecioVuelo);
            _comando.Parameters.AddWithValue("@FechaHoraSalida", V.FechaHoraSalida);
            _comando.Parameters.AddWithValue("@FechaHoraLlegada", V.FechaHoraLlegada);


            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);


            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - El vuelo no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - Verifique su vuelo");
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }

        }

        public List<Vuelo> ListarVuelo(Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Vuelo _unVuelo = null;
            List<Vuelo> listaVuelo = new List<Vuelo>();

            SqlCommand _comando = new SqlCommand("ListadoVuelo", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        _unVuelo = new Vuelo((string)_lector["IDvuelo"], (DateTime)_lector["FechaHoraSalida"], (DateTime)_lector["FechaHoraLlegada"], 
                            (byte)_lector["CantidadAsientos"], (double)_lector["PrecioVueo"],(Aeropuertos)_lector["IDAeropuertoLlegada"], (Aeropuertos)_lector["IDAeropuertoSalida"]);
                        
                        
                        listaVuelo.Add(_unVuelo);
                    } 
                }
                _lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }
            return listaVuelo;
        }
    }
}


