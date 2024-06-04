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
    internal class PersistenciaEmpleado : IPeristenciaEmpleado
    {

        //singleton 

        private static PersistenciaEmpleado _instancia = null;
        private PersistenciaEmpleado() { }
        public static PersistenciaEmpleado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaEmpleado();
            return _instancia;
        }


        //operaciones
        public Empleado LogueoEmpleado(string pUsuLog,string pContrasena)
        {
            {
                SqlConnection _cnn = new SqlConnection(Conexion.Cnn());
                Empleado _unEmpleado = null;

                SqlCommand _comando = new SqlCommand("Logueo", _cnn);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@UsuLog", pUsuLog);
                _comando.Parameters.AddWithValue("@Contrasena ", pContrasena);

                try
                {
                    _cnn.Open();
                    SqlDataReader _lector = _comando.ExecuteReader();
                    if (_lector.HasRows)
                    {
                        _lector.Read();
                        _unEmpleado = new Empleado((string)_lector["UsuLog"], (string)_lector["NombreCompleto"], (string)_lector["Contrasena"], (string)_lector["Labor"]);
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
                return _unEmpleado;
            }

        }

        public Empleado  BuscarEmpleado(string pUsuLog,Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Empleado unEmpleado = null;

            SqlCommand _comando = new SqlCommand("BuscarEmpleado", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@UsuLog", pUsuLog);
          

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    unEmpleado = new Empleado (pUsuLog , (string)_lector["NombreCompleto"],(string)_lector["Contrasena"], (string)_lector["Labor"]);
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
            return unEmpleado;
        }
    }
}

