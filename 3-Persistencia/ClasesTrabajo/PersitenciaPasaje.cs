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
    public class PersitenciaPasaje : Venta
    {
        // singleton
        private static PersitenciaPasaje _instancia = null;
        private PersitenciaPasaje() { }
        internal static PersitenciaPasaje GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersitenciaPasaje();
            return _instancia;
        }


        //operaciones
        internal void AltaPasaje(int NVenta, Pasaje NAsiento, SqlTransaction trn, Clientes clientes)

        {
            SqlCommand _comando = new SqlCommand("AltaPasaje", trn.Connection);
            _comando.CommandType = CommandType.StoredProcedure;


            _comando.Parameters.AddWithValue("@NVenta", NVenta);
            _comando.Parameters.AddWithValue("@NAsiento", NAsiento);
            _comando.Parameters.AddWithValue("@Clientes", clientes);
            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_Retorno);

            try
            {
                _comando.Transaction = trn;
                _comando.ExecuteNonQuery();


                int retorno = Convert.ToInt32(_Retorno.Value);
                if (retorno == -1)
                    throw new Exception("Pasaje Invalido");
                else if (retorno == 0)
                    throw new Exception("Error No Especificado");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        internal List<Venta> ListarPasajes(int NVenta)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());

            SqlCommand _comando = new SqlCommand("ListarPasajes", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NVenta", NVenta);
            Venta Pasaje = null;
            List<Venta> _ListaPasaje = new List<Venta>();

            try
            {
                
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();

                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        Pasaje _unPasaje = null;
                        _unPasaje = (PersitenciaPasaje.GetInstancia().ListarPasajes(int)_lector["NVenta"]);

                        if (_unPasaje == null)
                            _unPasaje = PersitenciaPasaje.GetInstancia().ListarPasajes(int)_lector["NVenta"]);

                        Pasaje = new Venta ((int)_lector["NVenta"], (DateTime)_lector["Fecha"], (double)_lector["Precio"],
                            (Clientes)_lector["IDCliente"], (Empleado)_lector["UsuLog"], (Vuelo)_lector["IDVuelo"],_ListaPasaje);
                        _ListaPasaje.Add(Pasaje);
                    }
                }

                _lector.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _cnn.Close();
            }
            return _ListaPasaje;
        }
    }
    }
    
}



