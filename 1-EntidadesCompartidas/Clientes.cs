using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EntidadesCompartidas
{
    public class Clientes
    {

        //atributos
        private string _IDPasaporte;
        private string _Nombre;
        private string _Contrasena;
        private Int64 _NTarjeta;



        //Propiedades
        public string IDPasaporte
        {
            get { return _IDPasaporte; }
            set { _IDPasaporte = value; }
        }

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }

        }

        public string Contrasena
        {
            get { return _Contrasena; }
            set { _Contrasena = value; }

        }

        public Int64 NTarjeta
        {
            get { return _NTarjeta; }
            set { _NTarjeta = value; }
        }


        public Clientes(string pIDPasaporte, string pNombre, string pContrasena, int pNTarjeta)
        {
            IDPasaporte = pIDPasaporte;
            Nombre = pNombre;
            Contrasena = pContrasena;
            NTarjeta = pNTarjeta;

        }

        public Clientes()
        {

        }

        public void ValidarClientes()
        {
            if (this.IDPasaporte.Trim().Length == 15)
                throw new Exception("El ID pasaporte debe de tener 15 caracteres");

            if (this.Nombre.Trim().Length <= 1 || Nombre.Trim().Length <= 100)
                throw new Exception("El Nombre del cliente  admite hasta 100 caracteres ");

            if (this.Contrasena.Trim().Length <= 1 || Contrasena.Trim().Length <= 10 )
                throw new Exception( " La contrasena debe estar comprendida entre 1 a 10 caracteres");

             if (this.NTarjeta.ToString().Length == 16)
                throw new Exception("El numero de tarjeta debe de presentar 16 caracteres ");
        }


    }
}





