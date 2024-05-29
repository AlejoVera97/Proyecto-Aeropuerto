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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("ClienteAlta", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@Npasaporte", pCliente.NPasaporte);
            _comando.Parameters.AddWithValue("@Nombre", pCliente.Nombre);
            _comando.Parameters.AddWithValue("@Contrasena", pCliente.Contrasena);
            _comando.Parameters.AddWithValue("@Ntarjeta", pCliente.NTarjeta);

            SqlParameter _ParmRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _ParmRetorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_ParmRetorno);

            SqlTransaction _miTransaccion = null;

            try
            {
                _cnn.Open();

                _miTransaccion = _cnn.BeginTransaction();

                _comando.Transaction = _miTransaccion;
                _comando.ExecuteNonQuery();


                int _NPasaporte = Convert.ToInt32(_ParmRetorno.Value);
                if (_NPasaporte == -1)
                    throw new Exception("Cliente existe");
                else if (_NPasaporte == -2)
                    throw new Exception("Error en el cliente");


                _miTransaccion.Commit();
            }
            catch (Exception ex)
            {
                _miTransaccion.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }

        }

        public void ModificarCliente(Clientes pCliente, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("ClienteModificar", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NPasaporte", pCliente.NPasaporte);
            _comando.Parameters.AddWithValue("@Nombre", pCliente.Nombre);
            _comando.Parameters.AddWithValue("@Contrasena", pCliente.Contrasena);
            _comando.Parameters.AddWithValue("@Ntarjeta", pCliente.NTarjeta);


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
                    throw new Exception("El cliente no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Modificacion del cliente");



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
        public void BajaCliente(Clientes pClientes, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("Clientes", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NPasaporte", pClientes.NPasaporte);
            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("El cliente tiene vuelos asociados");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("El Cliente tiene pasajes asociados");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error en Borrar Cliente");

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
        public Clientes BuscarCliente(string pNPasaporte,Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            Clientes _unCliente = null;

            SqlCommand _comando = new SqlCommand("BuscarCliente", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NPasaporte", pNPasaporte);

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

        internal void BuscarTodosClientes ( string  pNPasaporte)
        { }
        
        public List<Clientes> ListarCliente()
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            Clientes _unCliente = null;
            List<Clientes> _listaClientes = new List<Clientes>();

            SqlCommand _comando = new SqlCommand("ListadoClientes", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        _unCliente = new Clientes((string)_lector["NPasaporte"], (string)_lector["Nombre"], (string)_lector["Contrasena"], (int)_lector["NTarjeta"]);
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
