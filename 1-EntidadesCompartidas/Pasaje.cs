using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;


namespace EntidadesCompartidas
{
    public class Pasaje
    {
        private int _NAsiento;
        private Clientes _IDPasaporte;

        //propiedades

        [DisplayName("NAsiento")]
        public int NAsiento
        {

            get { return _NAsiento; }
            set { _NAsiento = value; }
           
        }

        [DisplayName("IDpasaporte")]
        public Clientes IDPasaporte
        {
            get { return _IDPasaporte; }
            set { _IDPasaporte = value; }
        }
             
       //constructores
        public Pasaje(int pNAsiento, Clientes pIDPasaporte)
        {
            NAsiento = pNAsiento;
            IDPasaporte = pIDPasaporte;
        }

        public Pasaje()
        {

        }

        public void ValidarPasaje()
        {
            if(this.NAsiento <= 1 || NAsiento <= 300)
                throw new Exception("El número de asiento debe estar entre 1 y 300");
            if (this.IDPasaporte == null )
                throw new Exception("Error en IDPasaporte invalido");


        }
    }
}


