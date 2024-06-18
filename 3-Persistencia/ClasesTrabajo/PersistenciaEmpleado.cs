using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;



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
                Empleado _unEmpleado = null;
                
                SqlConnection _cnn = new SqlConnection(Conexion._cnn());
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
                        string _NombreCompleto = _lector["Nombre"].ToString();
                        string _Labor = _lector["Labor"].ToString();

                        _unEmpleado = new Empleado(pUsuLog, _NombreCompleto,pContrasena, _Labor);
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

        public Empleado BuscarEmpleado(string pUsuLog, Empleado E)
        {
            Empleado unEmpleado = null;

            SqlConnection _cnn = new SqlConnection(Conexion._cnn(E));
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
                    string _NombreCompleto = _lector["Nombre"].ToString();
                    string _Labor = _lector["Labor"].ToString();
                    string _Contrasena = _lector["Contrasena"].ToString();

                    unEmpleado = new Empleado(pUsuLog,_NombreCompleto,_Contrasena,_Labor);
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

