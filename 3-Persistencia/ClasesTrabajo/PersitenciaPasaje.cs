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
    public class PersitenciaPasaje : Venta
    {
        // singleton
        private static PersitenciaPasaje _instancia = null;
        private PersitenciaPasaje() { }
        public static PersitenciaPasaje GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersitenciaPasaje();
            return _instancia;
        }


        //operaciones
        public void AltaPasaje(int NVenta, Pasaje NAsiento, SqlTransaction trn, Clientes clientes)
        {
            SqlCommand oComando = new SqlCommand("AltaPasaje ", trn.Connection);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _NVenta = new SqlParameter("@NVenta", NVenta);
            SqlParameter _NAsiento = new SqlParameter("@NAsiento", NAsiento);
            SqlParameter _Clientes = new SqlParameter("@Clientes", Clientes);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_NVenta);
            oComando.Parameters.Add(_NAsiento);
            oComando.Parameters.Add(_Clientes);
            oComando.Parameters.Add(_Retorno);

            int oAfectados = -1;

            try
            {
                oComando.Transaction = trn;
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("El pasaje no existe");
                if (oAfectados == -2)
                    throw new Exception("El numero de venta no existe");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Venta> ListarPasajes()
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);
            List<Pasaje> _listaPasaje = new List<Pasaje>();
            Venta _unVenta = null;

            SqlCommand _comando = new SqlCommand("ListaPasajes", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        //busco si hay venta

                        Venta _unaVenta = null;
                        _unaVenta = PersitenciaVenta.GetInstancia().AltaVenta((string)_lector["IDVenta"]);
                        if (_unaVenta == null)
                            _unaVenta = PersitenciaVenta.GetInstancia().AltaVenta((int)_lector["IDVenta"]);

                        //creo el movimiento
                        _unVenta = new Venta((int)_lector["IDventa"],


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
            return _lista;
        }
    }
}

