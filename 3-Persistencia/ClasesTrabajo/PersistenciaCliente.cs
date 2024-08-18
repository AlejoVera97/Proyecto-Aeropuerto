using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;




namespace Persistencia
{
    internal class PersistenciaCliente : IPersistenciaCliente
    {
        //singleton
        private static PersistenciaCliente _instancia = null;

        private PersistenciaCliente() { }

        public static PersistenciaCliente GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaCliente();
            return _instancia;
        }

        //operaciones
        public void AltaCliente(Clientes C, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("AltaCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDpasaporte", C.IDPasaporte);
            _comando.Parameters.AddWithValue("@Nombre", C.Nombre);
            _comando.Parameters.AddWithValue("@Contrasena", C.Contrasena);
            _comando.Parameters.AddWithValue("@Ntarjeta", C.NTarjeta);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            
            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("ERROR -  EL CLIENTE YA EXISTE CON EL IDPASAPORTE  ");
                            else if ((int)_retorno.Value == -2)
                    throw new Exception("ERROR - EN EL ALTA DEL CLIENTE ");
                
            
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

        public void ModificarCliente(Clientes C, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("ModificarCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDPasaporte", C.IDPasaporte);
            _comando.Parameters.AddWithValue("@Nombre", C.Nombre);
            _comando.Parameters.AddWithValue("@Contrasena", C.Contrasena);
            _comando.Parameters.AddWithValue("@Ntarjeta", C.NTarjeta);


            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);


            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - NO EXISTE UN CLIENTE CON EL IDPASAPORTE ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - EN LA  MODIFICACION DEL CLIENTE  ");
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

        public void BajaCliente(Clientes C, Empleado E)
        {

            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _Comando = new SqlCommand("BajaCliente", _cnn);
            _Comando.CommandType = CommandType.StoredProcedure;

            _Comando.Parameters.AddWithValue("@NumPasaporte", C.IDPasaporte);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;
            _Comando.Parameters.Add(_Retorno);

            try
            {
                _cnn.Open();
                _Comando.ExecuteNonQuery();

                if ((int)_Retorno.Value == -1)
                    throw new Exception("ERROR- NO EXISTE UN CLIENTE CON EL IDPASAPORTE");

                if ((int)_Retorno.Value == -2)
                    throw new Exception("ERROR  - NO SE PUEDE ELIMINAR");


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

        public Clientes BuscarCliente(string pIDPasaporte, Empleado E)
        {
            Clientes _unCliente = null;

            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("BuscarCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@NPasaporte", pIDPasaporte);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    string _Nombre = _lector["Nombre"].ToString();
                    string _Contrasena  = _lector["Contrasenia"].ToString();
                    int _NTarjeta = Convert.ToInt32(_lector["NTarjeta"]);

                    _unCliente = new Clientes (pIDPasaporte,_Nombre, _Contrasena,_NTarjeta);  
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
            return _unCliente;
        }

        internal Clientes BuscarTodosClientes(string pIDPasaporte, Empleado E)
        {
            Clientes _Cliente = null;


            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _comando = new SqlCommand("BuescarTodosClientes", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
           
            _comando.Parameters.AddWithValue("@IDPsaporte", pIDPasaporte);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    string _Nombre = _lector["Nombre"].ToString();
                    string _Contrasena = _lector["Contrasenia"].ToString();
                    int _NTarjeta = Convert.ToInt32(_lector["NTarjeta"]);

                    _Cliente = new Clientes (pIDPasaporte,_Nombre, _Contrasena,_NTarjeta);    
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
            return _Cliente;
        }
        
        public List<Clientes> ListarCliente(Empleado E)
        {
            Clientes _unCliente = null;
            List<Clientes> _ListaClientes = new List<Clientes>();
            
            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
           SqlCommand _comando = new SqlCommand("ListadoClientes", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        string _IDPasaporte = _lector["IDPasaporte"].ToString();
                        string _Nombre = _lector["Nombre"].ToString();
                        string _Contrasena = _lector["Contrasenia"].ToString();
                        int _NTarjeta = Convert.ToInt32(_lector["NTarjeta"]);

                        _unCliente = new Clientes(_IDPasaporte, _Nombre, _Contrasena,_NTarjeta);
                        _ListaClientes.Add(_unCliente); 

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
            return _ListaClientes;
        }

    }
}


