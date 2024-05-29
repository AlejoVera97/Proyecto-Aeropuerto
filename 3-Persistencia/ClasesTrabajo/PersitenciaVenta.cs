using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using EntidadesCompartidas;
using System.Collections;

//-------agregar usuings-----//
using System.Data.SqlClient;
using System.Data;
//---------------------------//


namespace Persistencia
{
    internal class PersitenciaVenta : IPersistenciaVenta

    {
        // singleton 
        private static PersitenciaVenta _instancia = null;
        private PersitenciaVenta() { }
        public static PersitenciaVenta GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersitenciaVenta();
            return _instancia;
        }


        // operaciones 

        public void AltaVenta(Venta pVenta, Empleado E)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("Venta", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@IDventa", pVenta.IDventa);
            _comando.Parameters.AddWithValue("@Precio", pVenta.Precio);
            _comando.Parameters.AddWithValue("@Fehca", pVenta.Fecha);

            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("La venta no existe ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Alta venta");
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
        public List<Venta> ListarVentas(Vuelo V, Empleado E)
        {
            {
                List<Venta> _ListaVenta = new List<ListarVentas>(V, E);

                SqlConnection _Conexion = new SqlConnection(Conexion.Cnn);
                SqlCommand _Comando = new SqlCommand("ListarVentas", _Conexion);
                _Comando.CommandType = CommandType.StoredProcedure;

                _Comando.Parameters.AddWithValue("@IDVuelo", V);
                _Comando.Parameters.AddWithValue("@UsuLog", E);

                SqlDataReader _Reader;
                try
                {
                    _Conexion.Open();
                    _Reader = _Comando.ExecuteReader();

                    while (_Reader.Read())
                    {
                        string _IDVuelo = (string)_Reader["IDVuelo"];
                        string _UsuLog = (string)_Reader["UsuLog"];
                        Venta v = new Venta().ValidarVenta(V);
                        Empleado e = new Empleado().ValidarEmpleado(E);
                        _ListaVenta.Add(v);
                    }

                    _Reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    _Conexion.Close();
                }

                return _ListaVenta;
            }

        }
    }
}
