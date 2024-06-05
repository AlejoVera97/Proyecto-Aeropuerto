using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EntidadesCompartidas
{
    public class Aeropuertos
    {


        private string _IDAeropuerto;
        private string _Nombre;
        private string _Direccion;
        private int _ImpuestoPartida;
        private int _ImpuestoLlegada;
        private Ciudad _ciudad;



        public string IDAeropuerto
        {
            get { return _IDAeropuerto; }
            set{_IDAeropuerto = value; }
            
            
        }

        public string Nombre
        {
            get { return _Nombre; }
            set {  _Nombre = value; }
            
        }
                public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value ; }
           
        }

        public int ImpuestoParitda
        {
            get { return _ImpuestoPartida; }
            set { _ImpuestoPartida = value;}

        }

        public int ImpuestoLlegada
        {
            get { return _ImpuestoLlegada; }
            set { _ImpuestoLlegada = value;}
            
        }

        public Ciudad _Ciudad
        {
            get { return _ciudad; }
            set { _ciudad = value; }
        }



        public Aeropuertos(string pIDAeropuerto, string pNombre, string pDireccion, int pImpuestoPartida,
            int pImpuestoLlegada, Ciudad pCiudad)

        {
            IDAeropuerto = pIDAeropuerto;
            Nombre = pNombre;
            Direccion = pDireccion;
            ImpuestoParitda = pImpuestoPartida;
            ImpuestoLlegada = pImpuestoLlegada;
            _Ciudad = pCiudad;



        }

        public Aeropuertos() { }

       
        public void ValidarAeropuerto()
        {
            if (this.IDAeropuerto.Trim().Length == 3)
                throw new Exception("El aeropuerto debe de tener exactamente  3 LETRAS");

            if (this.Nombre.Trim().Length <= 1 || Nombre.Trim().Length <= 100)  
                    throw new Exception("El Nombre del aeropuerto admite hasta 100 caracteres ");

            if (this.Direccion.Trim().Length <= 1 || Direccion.Trim().Length <= 255)
                throw new Exception("La direccion del aeropuerto admite hasta 255 caracteres ");

            if (this.ImpuestoParitda <= 0)
                throw new Exception("El impuesto de partida  debe ser positivo");

            if (this.ImpuestoLlegada <= 0)
                throw new Exception("El impuesto  de llegada debe ser positivo");

            if (this._Ciudad == null)
                throw new Exception("Error el IDCiudad no es valido");
        }

    }




}

