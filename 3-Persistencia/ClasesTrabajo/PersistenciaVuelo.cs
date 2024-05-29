using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using EntidadesCompartidas;

//-------agregar usuings-----//
using System.Data.SqlClient;
using System.Data;
//---------------------------//



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
        public void AltaVuelo(Vuelo pVuelo,Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("AltaVuelo", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IDVuelo", pVuelo.IDvuelo);
            _comando.Parameters.AddWithValue("@CantidadAsiento", pVuelo.CantidadAsientos);
            _comando.Parameters.AddWithValue("@PrecioVuelo", pVuelo.PrecioVuelo);
            _comando.Parameters.AddWithValue("@FechaHoraSalida", pVuelo.FechaHoraSalida);
            _comando.Parameters.AddWithValue("@FechaHoraLlegada", pVuelo.FechaHoraLlegada);

            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("El vuelo no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en vuelo");
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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            Vuelo unVuelo = null;
            List<Vuelo> listaVuelo = new List<Vuelo>();

            SqlCommand _comando = new SqlCommand("ListadoVuelo", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        unVuelo = new Vuelo((string)_lector["IDvuelo"], (DateTime)_lector["FechaHorasalida"], (DateTime)_lector["FechaHoraLlegada"],
                            (byte)_lector["CantidadAsientos"], (double)_lector["PrecioVuelo"], (Vuelo)_lector["IDAeropuertoSalida"], (Vuelo)_lector["IDAeropuertoLLegada"]);
                        
                        
                        listaVuelo.Add(unVuelo);
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


