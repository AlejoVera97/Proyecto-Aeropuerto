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
        public static PersitenciaPasaje GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersitenciaPasaje();
            return _instancia;
        }


        //operaciones
        internal void AltaPasaje(int NVenta, Pasaje pPasaje, SqlTransaction trn)

        {
            SqlCommand _Comando = new SqlCommand("AltaPasaje ", trn.Connection);
            _Comando.CommandType = CommandType.StoredProcedure;

            SqlParameter _NVenta = new SqlParameter("@NVenta", NVenta);
            SqlParameter _IDPasaporte = new SqlParameter("@IDPasaporte ", pPasaje.IDPasaporte);
            SqlParameter _NAsiento = new SqlParameter("@NAsiento", pPasaje.NAsiento);
           

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;
            _Comando.Parameters.Add(_Retorno);


            try
            {
                _Comando.Transaction = trn;
                _Comando.ExecuteNonQuery();
                int resultado = Convert.ToInt32(_Retorno.Value);

                if (resultado == -1)
                    throw new Exception("YA EXISTE ESE PASAJE CON NVENTA Y ASIENTO");

                if (resultado == -2)
                    throw new Exception("NO EXISTE LA VENTA");

                if (resultado == -3)
                    throw new Exception("NO EXISTE EL CLIENTE");

                if (resultado == -4)
                    throw new Exception("ERROR  - NO SE DA EL ALTA");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }

        public  List<Pasaje> ListarPasajes(int IDVenta,Empleado E)

        {

            Pasaje unPasaje = null;
            List<Pasaje> _Lista = new List<Pasaje>();

            SqlConnection _Cnn = new SqlConnection(Conexion._cnn(E));
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

        
        
    



    




