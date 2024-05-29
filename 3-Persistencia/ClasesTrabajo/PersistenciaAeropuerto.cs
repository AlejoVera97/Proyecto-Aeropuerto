using System;
using System.Collections.Generic;
using System.Linq;
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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand _comando = new SqlCommand("Alta Aeropuertos", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDAeropuerto);
            _comando.Parameters.AddWithValue("@Nombre", A.Nombre);
            _comando.Parameters.AddWithValue("@Direccion", A.Direccion);
            _comando.Parameters.AddWithValue("@ImpuestoLlegada", A.ImpuestoLlegada);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A.ImpuestoParitda);



            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;


            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - El Aeropuerto no existe ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - No se puede dar de alto el Aeropuerto");
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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("ModificarCiudad", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDAeropuerto);
            _comando.Parameters.AddWithValue("@Nombre", A.Nombre);
            _comando.Parameters.AddWithValue("@Direccion", A.Direccion);
            _comando.Parameters.AddWithValue("@ImpuestoLlegada", A.ImpuestoLlegada);
            _comando.Parameters.AddWithValue("@ImpuestoPartida", A.ImpuestoParitda);

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
                    throw new Exception("Error - El Aeropuerto no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - Modificacion de Aeropuerto");



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
        public static Aeropuertos BuscarAeropuerto(string pIDaeropuerto, Empleado E)

        {
            
                // Variables to hold airport data
                string _IDaeropuerto;
                string _nombre;
                string _direccion;
                int _impuestoLlegada;
                int _impuestoPartida;
                Ciudad _IDCiudad;

                // Variable to hold the result
                Aeropuertos a = null;

                // Establish a connection to the database
                SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
                SqlCommand oComando = new SqlCommand("SELECT * FROM Aeropuertos WHERE IDaeropuerto = @IDaeropuerto", oConexion);
                oComando.Parameters.AddWithValue("@IDaeropuerto", pIDaeropuerto);

                SqlDataReader oReader;

                try
                {
                    // Open the connection
                    oConexion.Open();

                    // Execute the command and get a data reader
                    oReader = oComando.ExecuteReader();

                    // Read the data
                    if (oReader.Read())
                    {
                        _IDaeropuerto = oReader["IDaeropuerto"].ToString();
                        _nombre = oReader["Nombre"].ToString();
                        _direccion = oReader["Direccion"].ToString();
                        _impuestoLlegada = Convert.ToInt32(oReader["ImpuestoLlegada"]);
                        _impuestoPartida = Convert.ToInt32(oReader["ImpuestoPartida"]);
                        _IDCiudad = new Ciudad(oReader["IDCiudad"].ToString());

                        // Create a new Aeropuertos object
                        a = new Aeropuertos(_IDaeropuerto, _nombre, _direccion, _impuestoPartida, _impuestoLlegada, _IDCiudad);
                    }

                    // Close the reader
                    oReader.Close();
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    throw new Exception(ex.Message);
                }
                finally
                {
                    // Close the connection
                    oConexion.Close();
                }

                // Return the result
                return a;
            }
        internal static Aeropuertos BuscarTodosAeropuertos( string IDaeropuerto)
        {
            //{
            //    string _IDaeropuerto;
            //    string _nombre;
            //    string _direccion;
            //    int _impuestoLlegada;
            //    int _impuestoPartida;
            //    Ciudad _IDCiudad = null;
            //    Aeropuertos _Aeropuerto = null;


            //    SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
            //    SqlCommand oComando = new SqlCommand("Buscar Aeropuerto " + pIDaeropuerto, oConexion);

            //    SqlDataReader oReader;

            //    try
            //    {
            //        oConexion.Open();
            //        oReader = oComando.ExecuteReader();

            //        if (oReader.Read())
            //        {
            //            _IDaeropuerto = (string)oReader["IDaeropuerto"];
            //            _nombre = (string)oReader["Nombre"];
            //            _direccion = (string)oReader["Direccion"];
            //            _impuestoLlegada = (int)oReader["ImpuestoLlegada"];
            //            _impuestoPartida = (int)oReader["ImpuestoPartida"];
            //            _IDCiudad = (string)oReader["IDCiudad"];
            //            a = new Aeropuertos(_IDaeropuerto, _nombre, _direccion, _impuestoPartida, _impuestoLlegada, _IDCiudad);
            //        }

            //        oReader.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message);
            //    }
            //    finally
            //    {
            //        oConexion.Close();
            //    }
            //    return a;
            //}


        }
        public void BajaAeropuerto(Aeropuertos A)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand _comando = new SqlCommand("EliminarAeropuerto", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter _nomu = new SqlParameter("@Aeropuerto", A.IDaeropuerto);

            _comando.Parameters.AddWithValue("@IDAeropuerto", A.IDaeropuerto);
            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            int _afectados = -1;


            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                _afectados = (int)_comando.Parameters["@Retorno"].Value;
                if (_afectados == -1)
                    throw new Exception("El Aeropuerto no existe - No se elimina");
                if (_afectados == -2)
                    throw new Exception("El Aeropuerto tiene vuelos asociados - No se elimina");
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
        public List<Aeropuertos> ListarAeropuerto()
        {


            List<Aeropuertos> _Lista = new List<Aeropuertos>();

            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            Aeropuertos unAeropuerto = null;
            List<Aeropuertos> _listarAeropuerto = new List<Aeropuertos>();

            SqlCommand _comando = new SqlCommand("ListadoAeropuerto", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        unAeropuerto = new Aeropuertos((string)_lector["IDAeropuerto"], (string)_lector["Nombre"], (string)_lector["Direccion"],
                            (int)_lector["ImpuestoPartida"], (int)_lector["ImpuestoLlegada"], (Ciudad)_lector["IDCiuad"]);
                        _listarAeropuerto.Add(unAeropuerto);
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
            return _listarAeropuerto;
        }

    }
}









