using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;



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
            SqlConnection _Cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _Comando = new SqlCommand("AltaVenta", _Cnn);
            _Comando.CommandType = CommandType.StoredProcedure;

            _Comando.Parameters.AddWithValue("@PrecioTotal", pVenta.Precio);
            _Comando.Parameters.AddWithValue("@CodVuelo", pVenta.Vuelo.IDvuelo);
            _Comando.Parameters.AddWithValue("@UsuLog", E.UsuLog);
            _Comando.Parameters.AddWithValue("@IDPasaporte", pVenta.Clientes.IDPasaporte);


            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;
            _Comando.Parameters.Add(_Retorno);

            SqlTransaction miTransaccion = null;

            try
            {
                _Cnn.Open();
                miTransaccion = _Cnn.BeginTransaction();
                _Comando.Transaction = miTransaccion;
                _Comando.ExecuteNonQuery();

                int NVenta = Convert.ToInt32(_Retorno.Value);

                if (NVenta == -1)
                    throw new Exception("NO EXISTE EL VUELO");

                if (NVenta == -2)
                    throw new Exception("NO EXISTE EL EMPLEADO");

                if (NVenta == -3)
                    throw new Exception("NO EXISTE EL CLIENTE");

                if (NVenta == -4)
                    throw new Exception("ERROR - NO SE DA EL ALTA");

                foreach (Pasaje unPasaje in pVenta.VentaLista)
                {
                    PersitenciaPasaje.GetInstancia().AltaPasaje(NVenta, unPasaje, miTransaccion);
                }

                miTransaccion.Commit();
            }
            catch (Exception ex)
            {
                miTransaccion.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _Cnn.Close();
            }
        }



        public List<Venta> ListarVentas(Vuelo V, Empleado E)
        {
            Venta unaVenta = null;
            List<Venta> _Lista = new List<Venta>();

            SqlConnection _Cnn = new SqlConnection(Conexion._cnn(E));
            SqlCommand _Comando = new SqlCommand("ListarVenta", _Cnn);
            _Comando.CommandType = CommandType.StoredProcedure;

            _Comando.Parameters.AddWithValue("@CodVuelo", V.IDvuelo);

            try
            {
                _Cnn.Open();

                SqlDataReader _Reader = _Comando.ExecuteReader();
                if (_Reader.HasRows)
                {
                    while (_Reader.Read())
                    {

                        int _NVenta = Convert.ToInt32(_Reader["NVenta"]);
                        double _Precio = Convert.ToInt32(_Reader["Precio"]);
                        DateTime _Fecha = Convert.ToDateTime(_Reader["Fecha"]);
                        string _UsuLog = _Reader["UsuLog"].ToString();
                        string _IDPasaporte = _Reader["UsuLog"].ToString();

                        Clientes pCliente = PersistenciaCliente.GetInstancia().BuscarCliente(_IDPasaporte, E);
                        List<Pasaje> pListarPasajes = PersitenciaPasaje.GetInstancia().ListarPasajes(_NVenta, E);
                        Empleado pEmpleado = PersistenciaEmpleado.GetInstancia().BuscarEmpleado(_UsuLog, E);

                        unaVenta = new Venta(_NVenta, _Fecha, _Precio, pCliente, pEmpleado, V, pListarPasajes);

                        _Lista.Add(unaVenta);
                    }

                }
                _Reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _Cnn.Close();
            }
            return _Lista;

        }
    }
}







