    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

//-------agregar usuings-----//
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Emit;
//---------------------------//


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
        public void AltaCliente(Clientes pCliente, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("ClienteAlta", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDpasaporte", pCliente.IDPasaporte);
            _comando.Parameters.AddWithValue("@Nombre", pCliente.Nombre);
            _comando.Parameters.AddWithValue("@Contrasena", pCliente.Contrasena);
            _comando.Parameters.AddWithValue("@Ntarjeta", pCliente.NTarjeta);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            
            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("ERROR - El nombre del cliente ya existe.");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("ERROR - No fue posible dar el alta el cliente");
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

        public void ModificarCliente(Clientes pCliente, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("ClienteModificar", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDPasaporte", pCliente.IDPasaporte);
            _comando.Parameters.AddWithValue("@Nombre", pCliente.Nombre);
            _comando.Parameters.AddWithValue("@Contrasena", pCliente.Contrasena);
            _comando.Parameters.AddWithValue("@Ntarjeta", pCliente.NTarjeta);


            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);


            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - El cliente no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error -  en Modificacion del cliente");
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

        public void BajaCliente(Clientes pClientes, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("Clientes", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDPasaporte", pClientes.IDPasaporte);
            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - El cliente tiene vuelos asociados");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - El Cliente tiene pasajes asociados");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error -  en Borrar Cliente");

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
                    _unCliente = new Clientes((string)_lector["IDPasaporte"], (string)_lector["Nombre"], (string)_lector["Contrasena"], (int)_lector["NTarjeta"]);

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
                    _C = new Clientes ((string)_lector["IDPasaporte"], (string)_lector["Nombre"], (string)_lector["Contrasena"], (int)_lector["NTarjeta"]);
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


