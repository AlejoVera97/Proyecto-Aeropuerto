using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Web;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Persistencia
{
    internal class PersitenciaPasaje : Venta
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
        internal void AltaPasaje(int NVenta, Pasaje pPasaje, SqlTransaction trn)

        {
            SqlCommand oComando = new SqlCommand("AltaPasaje ", trn.Connection);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _NVenta = new SqlParameter("@NVenta", NVenta);
            SqlParameter _IDPasaporte = new SqlParameter("@IDPasaporte ", pPasaje.IDPasaporte);
            SqlParameter _NAsiento = new SqlParameter("@NAsiento", pPasaje.NAsiento);
           

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(_NVenta);
            oComando.Parameters.Add(_IDPasaporte);
            oComando.Parameters.Add(_Retorno);

            int oAfectados = -1;

            try
            {
                oComando.Transaction = trn;
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("YA EXISTE UN PASAJE CON ESE NUMERO DE VENTA Y ASIENTO");
                if (oAfectados == -2)
                    throw new Exception("NO EXISTE LA VENTA");
                if (oAfectados == -3)
                    throw new Exception(" NO EXISTE EL CLIENTE");
                if (oAfectados == -4)
                    throw new Exception("ERROR INESPERADO -NO SE DA DE ALTA ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public  List<Pasaje> ListarPasajes(int IDVenta,Empleado E)

        {

            Pasaje unPasaje = null;
            List<Pasaje> _Lista = new List<Pasaje>();

            SqlConnection _Cnn = new SqlConnection(Conexion.Cnn(E));
            SqlCommand _Comando = new SqlCommand("ListarPasajes", _Cnn);
            _Comando.CommandType = CommandType.StoredProcedure;

            _Comando.Parameters.AddWithValue("@IDVenta", IDVenta);

            try
            {
                _Cnn.Open();

                SqlDataReader _Reader = _Comando.ExecuteReader();
                if (_Reader.HasRows)
                {
                    while (_Reader.Read())
                    {
                        string _IDPasaporte = _Reader["IDPasaporte"].ToString();
                        int _NAsiento = Convert.ToInt32(_Reader["NAsiento"]);

                        Clientes pCliente = PersistenciaCliente.GetInstancia().BuscarCliente(_IDPasaporte, E);

                        unPasaje = new Pasaje(_NAsiento, pCliente);

                        _Lista.Add(unPasaje);
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

        
        
    



    




