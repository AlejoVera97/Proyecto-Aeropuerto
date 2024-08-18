using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;





namespace Persistencia
{
    internal class PersistenciaAeropuerto : IPersistenciaAeropuerto
    {

        //singleton
        private static PersistenciaAeropuerto _instancia = null;
        private PersistenciaAeropuerto() { }
        public static PersistenciaAeropuerto GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaAeropuerto();
            return _instancia;
        }
                //operaciones
        public void AltaAeropuerto(Aeropuertos A, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("Alta Aeropuertos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDAeropuerto);
            _comando.Parameters.AddWithValue("@Nombre", A.Nombre);
            _comando.Parameters.AddWithValue("@Direccion", A.Direccion);
            _comando.Parameters.AddWithValue("@ImpuestoLlegada", A.ImpuestoLlegada);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A.ImpuestoParitda);
            _comando.Parameters.AddWithValue("@Ciudad", A._Ciudad.IDCiudad);



            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);


            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - NO EIXTE LA CIUDAD SELECCIONADA ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error -  YA EXISTE EL ID DEL AEROPUERTO ");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error -  NO SE PUEDE DAR DE ALTA ");



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

        public void BajaAeropuerto(Aeropuertos A,Empleado E )
        {
            {
                SqlConnection oConexion = new SqlConnection(Conexion._cnn(E));
                SqlCommand oComando = new SqlCommand("BajaAeropuerto", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter _IDAeropuerto = new SqlParameter("@IDAeropuerto", A.IDAeropuerto);

                SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                _Retorno.Direction = ParameterDirection.ReturnValue;

                oComando.Parameters.Add(_IDAeropuerto);
                oComando.Parameters.Add(_Retorno);

                int oAfectados = -1;        

                try
                {
                    oConexion.Open();
                    oComando.ExecuteNonQuery();
                    oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                    if (oAfectados == -1)
                        throw new Exception("ERROR - EL AEROPUERTO NO EXISTE CON EL ID SELECCIONADO");
                    if (oAfectados == -2)
                        throw new Exception("ERROR -  NO SE PUEDE ELIMINAR");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    oConexion.Close();
                }
            }
        }
        
        public void ModificarAeropuerto(Aeropuertos A, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("ModificarCiudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDAeropuerto);
            _comando.Parameters.AddWithValue("@Nombre", A.Nombre);
            _comando.Parameters.AddWithValue("@Direccion", A.Direccion);
            _comando.Parameters.AddWithValue("@ImpuestoLlegada", A.ImpuestoLlegada);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A.ImpuestoParitda);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A._Ciudad.IDCiudad);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - NO EXISTE EL AEROPUERTO CON ESE ID ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - LA CIUDAD PREVIAMENTE EN EL SISTEMA , NO SE MODIFICA ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - NO SE PUEDE MODIFICAR ");

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

        public  Aeropuertos BuscarAeropuerto(string pIDAeropuerto, Empleado E)

        {
            Aeropuertos unAeropuerto = null;

            SqlConnection _Cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _Comando = new SqlCommand("BuscarAeropuerto", _Cnn);
            _Comando.CommandType = CommandType.StoredProcedure;

            _Comando.Parameters.AddWithValue("@IDAeropuerto", pIDAeropuerto);

            try
            {
                _Cnn.Open();

                SqlDataReader _Reader = _Comando.ExecuteReader();
                if (_Reader.HasRows)
                {
                    _Reader.Read();
                    string _Nombre = _Reader["Nombre"].ToString();
                    string _Direccion = _Reader["Direccion"].ToString();
                    string _IDCiudad = _Reader["IDCiudad"].ToString();
                    int _ImpuestoPartida = Convert.ToInt32(_Reader["ImpuestoPartida"]);
                    int _ImpuestoLlegada = Convert.ToInt32(_Reader["ImpuestoLLegada"]);

                    Ciudad pCiudad = PersistenciaCiudad.GetInstancia().BuscarTodasCiudades(_IDCiudad,E);

                    unAeropuerto = new Aeropuertos(pIDAeropuerto, _Nombre, _Direccion ,_ImpuestoPartida, _ImpuestoLlegada, pCiudad);
                }
                _Reader.Close();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                _Cnn.Close();

            }
            return unAeropuerto;
        }

        internal  Aeropuertos BuscarTodosAeropuertos( string pIDAeropuerto,Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            Aeropuertos _A= null;
            SqlCommand _comando = new SqlCommand("BuscarTodosAeropuertos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDAeropuerto", pIDAeropuerto);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    string _Nombre = _lector["Nombre"].ToString();
                    string _Direccion = _lector["Direccion"].ToString();
                    string _IDCiudad = _lector["IDCiudad"].ToString();
                    int _ImpuestoPartida = Convert.ToInt32(_lector["ImpuestoPartida"]);
                    int _ImpuestoLlegada = Convert.ToInt32(_lector["ImpuestoLlegada"]);

                    Ciudad pCiudad = PersistenciaCiudad.GetInstancia().BuscarCiudad(_IDCiudad, E);

                    _A = new Aeropuertos(pIDAeropuerto, _Nombre, _Direccion, _ImpuestoPartida, _ImpuestoLlegada, pCiudad);


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }
            return _A;
        }
    
        public List<Aeropuertos> ListarAeropuerto(Empleado E)
        {


            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            Aeropuertos _unA = null;
            List<Aeropuertos> _lista = new List<Aeropuertos>();

            SqlCommand _comando = new SqlCommand("ListadoAeropuerto", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        string _IDAeropuerto = _lector["IDAeropuerto"].ToString();
                        string _Nombre = _lector["Nombre"].ToString();
                        string _Direccion = _lector["Direccion"].ToString();
                        string _IDCiudad = _lector["IDCiudad"].ToString();
                        int _ImpuestoPartida = Convert.ToInt32(_lector["ImpuestoPartida"]);
                        int _ImpuestoLlegada = Convert.ToInt32(_lector["ImpuestoLlegada"]);

                        Ciudad pCiudad = PersistenciaCiudad.GetInstancia().BuscarTodasCiudades(_IDCiudad, E);

                        _unA = new Aeropuertos(_IDAeropuerto, _Nombre, _Direccion, _ImpuestoPartida, _ImpuestoLlegada, pCiudad);


                        _lista.Add(_unA);
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
            return _lista;
        }

    }
}









