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

        public void AltaVenta(Venta V, Empleado E)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn(E));
            SqlCommand oComando = new SqlCommand("AltaVenta", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _IDVenta = new SqlParameter("@IDVenta", V.IDventa);
            SqlParameter _Precio = new SqlParameter("@Precio ", V.Precio);
            SqlParameter _Cliente = new SqlParameter("@Cliente", V.Clientes.IDPasaporte);
            SqlParameter _Empleado = new SqlParameter("@Empleado ", V.Empleado.UsuLog);
            SqlParameter _Vuelo = new SqlParameter("@Vuelo", V.Vuelo.IDvuelo);
            SqlParameter _Lista = new SqlParameter("@Lista ", V.VentaLista);



            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_IDVenta);
            oComando.Parameters.Add(_Precio);
            oComando.Parameters.Add(_Cliente);
            oComando.Parameters.Add(_Empleado);
            oComando.Parameters.Add(_Vuelo);
            oComando.Parameters.Add(_Lista);

            int oAfectados = -1;
            SqlTransaction _transaccion = null;

            try
            {
                oConexion.Open();
                _transaccion = oConexion.BeginTransaction();
                oComando.Transaction = _transaccion;

                oComando.ExecuteNonQuery();

                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("ERROR- EL EMPLEADO NO COINCIDE ");
                if (oAfectados == -2)
                    throw new Exception("ERROR - EL CLIENTE NO COINCIDE ");
                if (oAfectados == -3)
                    throw new Exception("ERROR - EL VUELO NO EXISTE ");
                if (oAfectados == -4)
                    throw new Exception("ERROR -  EN EL PROCESO DE ALTA VENTA   ");
          
                _transaccion.Commit();
            }
            catch (Exception ex)
            {
                _transaccion.Rollback();
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
        }



        public List<Venta> ListarVentas(Vuelo V, Empleado E)
        {

            List<Venta> _Lista = new List<Venta>();

            SqlConnection _Conexion = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _Comando = new SqlCommand("ListarVentas", _Conexion);
            _Comando.CommandType = CommandType.StoredProcedure;
            _Comando.Parameters.AddWithValue("@Vuelo", V.IDvuelo);

            SqlDataReader _Reader;
            try
            {
                _Conexion.Open();
                _Reader = _Comando.ExecuteReader();

                while (_Reader.Read())
                {
                    int _IDVenta = (int)_Reader["IDVenta"];
                    DateTime _fecha = Convert.ToDateTime(_Reader["Fecha"]);
                    double _Precio = (double)_Reader["Precio"];
                    string _UsuLog = (string)_Reader["UsuLog"];
                    string _IDVuelo = (string)_Reader["IDVuelo"];
                    string _IDPasaporte = (string)_Reader["IDPasaporte"];


                    //Venta v = new Venta(_IDVenta, _fecha, _Precio, _UsuLog, _IDPasaporte, _IDVuelo, new PersitenciaVenta().ListarVentas(V, E));
                        
                    //_Lista.Add(v);

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

            return _Lista;
        }
    }
}


    
    
