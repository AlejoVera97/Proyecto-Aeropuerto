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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _comando = new SqlCommand("Venta", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@IDventa", pVenta.IDventa);
            _comando.Parameters.AddWithValue("@Precio", pVenta.Precio);
            _comando.Parameters.AddWithValue("@Fehca", pVenta.Fecha);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - La venta no existe ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error -  En Alta venta");
               
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
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(E));
            Venta _unaV = null;
            List<Venta> _lista = new List<Venta>();

            SqlCommand _comando = new SqlCommand("ListarVentas", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("Vuelo",V);


            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();

                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        Vuelo V = new Vuelo(); // me tranque aca...
                        _lista.Add(V);

                    }

                    _lector.Close();
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

            return _lista;

        }
    }

}

    
    
