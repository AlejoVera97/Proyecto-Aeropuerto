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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("Alta Aeropuertos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDAeropuerto);
            _comando.Parameters.AddWithValue("@Nombre", A.Nombre);
            _comando.Parameters.AddWithValue("@Direccion", A.Direccion);
            _comando.Parameters.AddWithValue("@ImpuestoLlegada", A.ImpuestoLlegada);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A.ImpuestoParitda);
            _comando.Parameters.AddWithValue("@IDCiudad", A.IDCiudad);



            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - El Aeropuerto no existe ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - No se puede dar de alta el Aeropuerto");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error - El nombre del aeropuerto ya existe");

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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("EliminarAeropuerto", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
                       
            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDAeropuerto);
            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);


            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - El Aeropuerto que intenta eliminar no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("ERROR - El aeropuerto que desea eliminar tiene viajes asociados y no es posible su eliminación.");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("ERROR - En eliminación del aeropuerto");
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
        
        public void ModificarAeropuerto(Aeropuertos A, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("ModificarCiudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDAeropuerto);
            _comando.Parameters.AddWithValue("@Nombre", A.Nombre);
            _comando.Parameters.AddWithValue("@Direccion", A.Direccion);
            _comando.Parameters.AddWithValue("@ImpuestoLlegada", A.ImpuestoLlegada);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A.ImpuestoParitda);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A.IDCiudad);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - El Aeropuerto que intenta modificar no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - En la modificacion de Aeropuerto");
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

        public  Aeropuertos BuscarAeropuerto(string pIDaeropuerto, Empleado E)

        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Aeropuertos _unA = null;

            SqlCommand _comando = new SqlCommand("BuscarAeropuerto", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@unA", pIDaeropuerto);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    if (_lector.Read())
                        _unA = new Aeropuertos (pIDaeropuerto, (string)_lector["Nombre"], (string)_lector["Direccion"], (int)_lector["ImpuestoPartida"],
                            (int )_lector["ImpuestoLlegada"] , (Ciudad)_lector["IDCiudad"]);
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
            return _unA;
        }

        internal  Aeropuertos BuscarTodosAeropuertos( string pIDaeropuerto)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());
            Aeropuertos _A= null;

            SqlCommand _comando = new SqlCommand("BuscarTodosAeropuertos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IDaeropuerto", pIDaeropuerto);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    _A = new Aeropuertos ((string)_lector["IDaeropuerto"], (string)_lector["Nombre"], (string)_lector["Direccion"],
                        (int)_lector["ImpuestoLlegada"], (int)_lector["ImpuestoPartida"], (Ciudad)_lector["IDCiuad"]);
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


            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
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
                        _unA = new Aeropuertos((string)_lector["IDAeropuerto"], (string)_lector["Nombre"], (string)_lector["Direccion"],
                            (int)_lector["ImpuestoPartida"], (int)_lector["ImpuestoLlegada"], (Ciudad)_lector["IDCiuad"]);
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









