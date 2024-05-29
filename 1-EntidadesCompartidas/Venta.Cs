using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


//-------------------------------------------
using System.ComponentModel;

//--------------------------------------------

namespace EntidadesCompartidas
{
    public class Venta
    {


        private int _IDVenta;
        private DateTime _Fecha;
        private double _Precio;
        private Clientes _Cliente;
        private Empleado _Empleado;
        private Vuelo _Vuelo;
        private List<Pasaje> _VentaListas;

        [DisplayName("Numero")]
        public int IDventa
        {
            get { return _IDVenta; }
            set { _IDVenta = value; }
        }

        [DisplayName("Precio")]
        public double Precio
        {
            get { return _Precio; }
            set { _Precio = value; }
        }



        
        public DateTime Fecha
        {
            get
            {
                return _Fecha;
            }

            set
            {
                _Fecha = value;
            }
        }

        [DisplayName("Clientes")]
        public Clientes Clientes
        {
            get { return _Cliente; }
            set { _Cliente = value; }

        }

        [DisplayName("Empleado")]
        public Empleado Empleado
        {
            get { return _Empleado; }
            set { _Empleado = value; }

        }
        public Vuelo Vuelo
        { get { return _Vuelo; }
          set { _Vuelo = value; }   
        }       

        public List<Pasaje> VentaLista
        {
            get { return _VentaListas; }
            set { _VentaListas= value; }
        }





        //constructor
        public Venta  ( int pIDVenta,DateTime pFecha,  double pPrecio, Clientes pCliente, Empleado pEmpleado, Vuelo pVuelo,List<Pasaje>VentasLista)
        {
            IDventa = pIDVenta;
            Fecha = pFecha;
            Precio = pPrecio;
            Clientes = pCliente;
            Empleado = pEmpleado;
            Vuelo = pVuelo;
            VentaLista = VentasLista;
                 
            
        }
        public Venta()
    {

    }
        public void ValidarVenta()
        {
            
            if (this.Precio <= 0 )
                throw new Exception("El precio debe ser positivo");
            if (this.Clientes==null)
                    throw new Exception("El ID pasaporte no es valido"); 
                
            if (this.Empleado==null)
                throw new Exception("El empleado no existe");

            if (this.Vuelo == null)
                throw new Exception("El Vuelo no es valido");

            if (this._VentaListas == null || this.VentaLista.Count == 0)
                throw new Exception( "Lista de Pasajes que se venden");
        }

    }
}


    



