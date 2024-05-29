using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace EntidadesCompartidas
{
    public class Vuelo
    {


        private string _IDvuelo;
        private DateTime _FechaHoraSalida;
        private DateTime _FechaHoraLlegada;
        private int _CantidadAsientos;
        private double _PrecioVuelo;
        private Aeropuertos _IDAeropuertoSalida;
        private Aeropuertos _IDAeropuertoLLegada;





        public string IDvuelo
        {
            get { return _IDvuelo; }
            set { _IDvuelo = value; }
        }

        public DateTime FechaHoraSalida
        {
            get { return _FechaHoraSalida; }
            set { _FechaHoraSalida =value; }    
        }

        public DateTime FechaHoraLlegada
        {
            get { return _FechaHoraLlegada; }
            set { _FechaHoraLlegada = value; }
        }

        public int CantidadAsientos
        {
            get { return _CantidadAsientos; }
            set { _CantidadAsientos = value; }
        }

        public double PrecioVuelo
        {
            get { return _PrecioVuelo; }
            set { _PrecioVuelo = value; }

        }

        public Aeropuertos IDAeropuertoSalida
        {
            get { return _IDAeropuertoSalida; }
            set { _IDAeropuertoSalida = value; }
        }

        public Aeropuertos IDAeropuertoLlegada
        {
            get { return _IDAeropuertoLLegada; }
            set { _IDAeropuertoLLegada = value; }
        }



        public Vuelo(string pIDvuelo, DateTime pFechaHoraSalida, DateTime pFechaHoraLlegada, byte pCantidadAsientos, double pPrecio,
                Aeropuertos pIDAeropuertoLlegada, Aeropuertos pIDAeropuertoSalida)

        {
            IDvuelo = pIDvuelo;
            FechaHoraLlegada = pFechaHoraLlegada;
            FechaHoraSalida = pFechaHoraSalida;
            CantidadAsientos = pCantidadAsientos;
            PrecioVuelo = pPrecio;
            IDAeropuertoLlegada = pIDAeropuertoLlegada;
            IDAeropuertoSalida = pIDAeropuertoSalida;

        }

        public Vuelo()
        {
        }

        public void ValidarVuelo()
        {
            if (this.IDvuelo.Trim().Length != 16) // REGEX y fecha de llegada tiene que posterir a la salida (validacion)
                throw new Exception("El ID vuelo debe de presentar 16 caracteres");

            if (this.CantidadAsientos < 100 || this.CantidadAsientos > 300)
                throw new Exception("La cantidad de asientos debe estar entre 100 y 300.");

            if (this.PrecioVuelo <= 1)
                throw new Exception(" El precio del vuelo debe ser positivo");

            if (this.IDAeropuertoLlegada == null)
                throw new Exception("El ID del aeropuerto de llegada  no es valido");

            if (this.IDAeropuertoSalida == null)
                throw new Exception("El ID del aeropuerto  de partida no es valido");
        }
    }

}




    









