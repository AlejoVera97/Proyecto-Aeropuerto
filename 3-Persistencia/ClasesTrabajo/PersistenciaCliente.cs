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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
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
                    throw new Exception("ERROR -  EL CLIENTE YA  EXISTE ");
                            else if ((int)_retorno.Value == -2)
                    throw new Exception("ERROR - EL CLIENTE NO SE PUDO DAR DE ALTA ");
                
            
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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("ClienteModificar", _cnn);
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
                    throw new Exception("Error - EL CLIENTE QUE INTENTA MODIFICAR NO EXISTE");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error -  MODIFICACION DE CLIENTE  ");
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

            SqlConnection oConexion = new SqlConnection(Conexion.Cnn(E));
            SqlCommand oComando = new SqlCommand("BajaCliente", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _IDPasaporte = new SqlParameter("@IDPasaporte", C.IDPasaporte) ;

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_IDPasaporte);
            oComando.Parameters.Add(_Retorno);

            int oAfectados = -1;

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("ERROR - EL CLIENTE NO EXISTE ");
                if (oAfectados == -2)
                    throw new Exception("ERROR - EL CLIENTE TIENE VENTAS ASOCIACADAS, NO SE PUEDE ELIMINAR" );
                if (oAfectados == -3)
                    throw new Exception("ERROR - EL CLIENTE TIENE VUELOS ASOCIADOS, NO SE PUEDE ELIMINAR");

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

        public Clientes BuscarCliente(string pIDPasaporte, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Clientes _unCliente = null;

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
                    _unCliente = new Clientes((string)_lector["IDPasaporte"], (string)_lector["Nombre"], (string)_lector["Contrasena"],
                        (int)_lector["NTarjeta"]);

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
            return _unCliente;
        }

        internal Clientes BuscarTodosClientes(string pIDPasaporte)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());
            Clientes _C = null;

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
                    _C = new Clientes ((string)_lector["IDPasaporte"], (string)_lector["Nombre"], (string)_lector["Contrasena"], 
                        
                        (int)_lector["NTarjeta"]);
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
            return _C;
        }
        
        public List<Clientes> ListarCliente(Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Clientes _unCliente = null;
            List<Clientes> _listaClientes = new List<Clientes>();

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
                        _unCliente = new Clientes((string)_lector["IDPasaporte"], (string)_lector["Nombre"], (string)_lector["Contrasena"], (int)_lector["NTarjeta"]);
                        _listaClientes.Add(_unCliente);
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
            return _listaClientes;
        }

    }
}


