using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;



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
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("AltaCiudad", _cnn);
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
                    throw new Exception("Error -  LA CIUDAD YA EXISTE CON SU ID   ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - EN EL ALTA DE LA CIUDAD ");

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

            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _Comando = new SqlCommand("BajaCiudad", _cnn);
            _Comando.CommandType = CommandType.StoredProcedure;

            SqlParameter _IDCiudad = new SqlParameter("@IDCiudad", C.IDCiudad);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;
            
            _Comando.Parameters.Add(_Retorno);

           try
            {
                _cnn.Open();
                _Comando.ExecuteNonQuery();

                if ((int)_Retorno.Value == -1)
                    throw new Exception("ERROR - EL ID CIUDAD NO EXISTE");

                if ((int)_Retorno.Value == -2)
                    throw new Exception("ERROR - LA CIUAD  NO SE PUEDE  ELIMINA");


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _cnn.Close();
            }
        }

        public Ciudad BuscarCiudad(string IDCiudad , Empleado E)
        {
            Ciudad _unaC = null;
           
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("@BuscarCiudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
           
            _comando.Parameters.AddWithValue("@IDCiudad", IDCiudad);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    string _NombrePais = _lector["NombrePais"].ToString();
                    string _NombreCiudad = _lector["NombreCiudad"].ToString() ; 

                    _unaC = new Ciudad(IDCiudad,_NombreCiudad,_NombrePais);

                  
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
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("ModificarCiudad", _cnn);
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
                    throw new Exception("Error - EL ID DE LA CIUDAD NO EXISTE ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - EN LA MODIFICACION DE CIUDAD ");
                
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

        internal Ciudad BuscarTodasCiudades (string pIDCiudad,Empleado E)
        {

            Ciudad _unaC= null;

            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("BuscarTodasCiudades", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
           
            _comando.Parameters.AddWithValue("@IDCiudad", pIDCiudad);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    string _NombrePais = _lector["NombrePais"].ToString();
                    string _NombreCiudad = _lector["NombreCiudad"].ToString();

                    _unaC = new Ciudad(pIDCiudad, _NombreCiudad, _NombrePais);

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
            Ciudad unaCiudad = null;
            List<Ciudad> _listaCiudad = new List<Ciudad>();

            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("ListarCiudad", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        
                        string _IDCiudad = _lector["IDCiudad"].ToString();
                        string _NombrePais = _lector["NombrePais"].ToString();
                        string _NombreCiudad = _lector["NombreCiudad"].ToString();

                        unaCiudad = new Ciudad(_IDCiudad,_NombreCiudad, _NombrePais);
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

