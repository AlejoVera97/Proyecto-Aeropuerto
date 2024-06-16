using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;





namespace Persistencia
{
    internal class PersitenciaPasaje : Venta
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
        internal void AltaPasaje(int NVenta, Pasaje NAsiento, SqlTransaction trn, Clientes C)

        {
            SqlCommand oComando = new SqlCommand("AltaPasaje ", trn.Connection);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _NVenta = new SqlParameter("@NVenta", NVenta);
            SqlParameter _NAsiento = new SqlParameter("@NAsiento", NAsiento);
            SqlParameter _IDPasaporte = new SqlParameter("@Cliente", C.IDPasaporte);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_NVenta);
            oComando.Parameters.Add(_NAsiento);
            oComando.Parameters.Add(_IDPasaporte);
            oComando.Parameters.Add(_Retorno);

            int oAfectados = -1;

            try
            {
                oComando.Transaction = trn;
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("LA VENTA DEL PASAJE DE VENTA NO EXISTE");
                if (oAfectados == -2)
                    throw new Exception("EL CLIENTE QUE OBTIENE EL PASAJE NO EXISTE");
                if (oAfectados == -3)
                    throw new Exception(" El ASIENTO YA ESTA VENDIDO ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal List<Venta> ListarPasajes(int NVenta)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn());
            Venta _Pasaje = null;
            List<Venta> _ListaPasaje = new List<Venta>();


            SqlCommand _comando = new SqlCommand("ListarPasajes", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NVenta", NVenta);
            _comando.Parameters.AddWithValue("@IDPasaporte", Clientes.IDPasaporte);


            try
            {

                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();

                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        int _NAsiento = Convert.ToInt32(_lector["NAsiento"]);
                        string _IDPasaporte = _lector["IDPasaporte"].ToString();
                        string _UsuLog = _lector["UsuLog"].ToString();




                        _Pasaje = new Pasaje(_NAsiento, (PersistenciaCliente.GetInstancia().BuscarCliente
                            (_IDPasaporte, E)));
                       

                        _ListaPasaje.Add(_Pasaje);
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

    




