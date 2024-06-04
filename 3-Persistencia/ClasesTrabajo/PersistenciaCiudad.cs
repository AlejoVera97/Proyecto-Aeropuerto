using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using System.Data.SqlClient;


//-------agregar usuings-----//
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Emit;
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
        public void AltaCiudad(Ciudad C, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("Ciudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDCiudad", C.IDCiudad);
            _comando.Parameters.AddWithValue("@NombreCiudad", C.NombreCiudad);
            _comando.Parameters.AddWithValue("@NombrePais", C.NombrePais);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("La ciudad no existe ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Alta Ciudad");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("El nombre de la ciudad ya existe");
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

        public void BajaCiudad(Ciudad C, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("Ciudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;



            _comando.Parameters.AddWithValue("@IDciudad", C.IDCiudad);
            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - La ciudad que intenta eliminar no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - En borrar la ciudad");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error - La ciudad que intenta eliminar tiene aeropuertos asociadios");
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

        public Ciudad BuscarCiudad(string C, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Ciudad _unaC = null;

            SqlCommand _comando = new SqlCommand("SP_BuscarUsuario", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@Usu", C);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    if (_lector.Read())
                        _unaC = new Ciudad (C , (string)_lector["NombreCiudad"], (string)_lector["NombrePais"]);
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
            return _unaC;
        }
    
        public void ModificarCiudad(Ciudad C, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("ModificarCiudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDCiudad", C.IDCiudad);
            _comando.Parameters.AddWithValue("@NombreCiudad", C.NombreCiudad);
            _comando.Parameters.AddWithValue("@NobrePais", C.NombrePais);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("La ciudad no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Modificacion de ciudad");
                
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

        internal Ciudad BuscarTodasCiudades (string IDCiudad)
        {

            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());
            Ciudad _unaC= null;

            SqlCommand _comando = new SqlCommand("BuscarTodasCiudades", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IDCiudad", IDCiudad);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    _unaC = new Ciudad(IDCiudad, (string)_lector["NombreCiudad"], (string)_lector["NombrePais"]);
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
            return _unaC;
        }
    
       
        public List<Ciudad> ListarCiudad(Empleado E )
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Ciudad unaCiudad = null;
            List<Ciudad> _listaCiudad = new List<Ciudad>();

            SqlCommand _comando = new SqlCommand("ListadoCiudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

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

