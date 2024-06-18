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
        public void AltaVuelo(Vuelo V, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("AltaVuelo", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@IDVuelo", V.IDvuelo);
            _comando.Parameters.AddWithValue("@CantidadAsiento", V.CantidadAsientos);
            _comando.Parameters.AddWithValue("@PrecioVuelo", V.PrecioVuelo);
            _comando.Parameters.AddWithValue("@FechaHoraSalida", V.FechaHoraSalida);
            _comando.Parameters.AddWithValue("@FechaHoraLlegada", V.FechaHoraLlegada);
            _comando.Parameters.AddWithValue("@AeropuertoLlegada", V.AeropuertoLlegada.ImpuestoLlegada);
            _comando.Parameters.AddWithValue("@AeropuertoSalida", V.AeropuertoSalida.ImpuestoParitda);


            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);


            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("ERROR- EL VUELO YA EXISTE CON ESE ID ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("ERROR - EL AEROPUERTO DE PARTIDA NO EXISTE  ");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("ERROR - EL AEROPUERTO DE SALIDA NO EXISTE ");
                else if ((int)_retorno.Value == -4)
                    throw new Exception("ERROR - DUARTE EL PROCESO DE ALTA VUELO");

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
            Vuelo _unVuelo = null;
            List<Vuelo> listaVuelo = new List<Vuelo>();

            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
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
                        string _IDVuelo = _lector["IDVuelo"].ToString();
                        DateTime _FechaPartida = Convert.ToDateTime(_lector["FechaPartida"]);
                        DateTime _FechaLlegada = Convert.ToDateTime(_lector["FechaLlegada"]);
                        double _Precio = Convert.ToInt32(_lector["Precio"]);
                        int  _CantidadAsiento = Convert.ToInt32(_lector["CantidadAsiento"]);
                        string _IDAeropuertoPartida =_lector["AeropuertoPartida"].ToString();
                        string _IDAeropuertoLlegada = _lector["AeropuertoLlegada"].ToString();

                        Aeropuertos _AeropuertoPartida = PersistenciaAeropuerto.GetInstancia().BuscarTodosAeropuertos(_IDAeropuertoPartida, E);
                        Aeropuertos _AeropuertoLlegada = PersistenciaAeropuerto.GetInstancia().BuscarTodosAeropuertos(_IDAeropuertoLlegada, E);

                        _unVuelo = new Vuelo(_IDVuelo, _FechaPartida, _FechaLlegada, _CantidadAsiento, _Precio, _AeropuertoLlegada, _AeropuertoPartida);

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


