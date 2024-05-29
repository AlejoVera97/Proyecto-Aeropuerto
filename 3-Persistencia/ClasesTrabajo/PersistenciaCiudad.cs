using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using System.Data.SqlClient;


//-------agregar usuings-----//
using System.Data.SqlClient;
using System.Data;
//---------------------------//


namespace Persistencia
{
    internal class PersistenciaCiudad : IPersistenciaCiudad


    {
        //singleton
        private static PersistenciaCiudad _instancia = null;
        private PersistenciaCiudad() { }
        public static PersistenciaCiudad GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaCiudad();
            return _instancia;
        }

        //operaciones
        public void AltaCiudad(Ciudad pCiudad, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand _comando = new SqlCommand("Ciudad", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDCiudad", pCiudad.IDCiudad);
            _comando.Parameters.AddWithValue("@NombreCiudad", pCiudad.NombreCiudad);
            _comando.Parameters.AddWithValue("@NombrePais", pCiudad.NombrePais);

            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("La ciudad no existe ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Alta Ciudad");
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
        public void BajaCiudad(Ciudad pCiudad, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("Ciudad", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IDciudad", pCiudad.IDCiudad);

            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("La ciudad tiene aeropuertos asociados");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Borrar la ciudad");


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
        public Ciudad BuscarCiudad(string pIDCiudad,Empleado E)
        {
            string _Ciudad;
            string _NombrePais;
            string _NombreCiudad;
            Ciudad C = null;

            SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
            SqlCommand oComando = new SqlCommand("Exec Buscar Ciudad '" + pIDCiudad + "'", oConexion);

            SqlDataReader oReader;

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.Read())
                {
                    _Ciudad = (string)oReader["IDCiudad"];
                    _NombrePais = (string)oReader["NombrePais"];
                    _NombreCiudad = (string)oReader["NombreCiudad"];
                    C = new Ciudad(_Ciudad,_NombrePais,_NombreCiudad);
                }

                oReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }
            return  C ;
        }
        public void ModificarCiudad(Ciudad pCiudad, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("ModificarCiudad", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IDCiudad", pCiudad.IDCiudad);
            _comando.Parameters.AddWithValue("@NombreCiudad", pCiudad.NombreCiudad);
            _comando.Parameters.AddWithValue("@NobrePais", pCiudad.NombrePais);




            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            SqlTransaction _miTransaccion = null;

            try
            {
                _cnn.Open();

                _miTransaccion = _cnn.BeginTransaction();

                _comando.Transaction = _miTransaccion;
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("La ciudad no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Modificacion de ciudad");



                _miTransaccion.Commit();
            }
            catch (Exception ex)
            {
                _miTransaccion.Rollback();
                throw ex;
            }
            finally
            {
                _cnn.Close();
            }
        }

        internal void BuscarTodasCiudades (string NPasaporte)
        { }
       
        public List<Ciudad> ListarCiudad()
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            Ciudad unaCiudad = null;
            List<Ciudad> _listaCiudad = new List<Ciudad>();

            SqlCommand _comando = new SqlCommand("ListadoCiudad", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        unaCiudad = new Ciudad((string)_lector["IDCiudad"], (string)_lector["NombreCiudad"], (string)_lector["NombrePais"]);
                        _listaCiudad.Add(unaCiudad);
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
            return _listaCiudad;
        }

    }
}

