using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EntidadesCompartidas
{
    public class Ciudad
    {
        //atributos
        private string _IDCiudad;
        private string _NombreCiudad;
        private string _NombrePais;



        //propiedades
        public string IDCiudad
        {
            get { return _IDCiudad; }
            set { _IDCiudad = value; }
        }
        public string NombreCiudad
        {
            get { return _NombreCiudad; }

            set { _NombreCiudad = value; }

        }
        public string NombrePais
        {
            get { return _NombrePais; }

            set { _NombrePais = value; }
        }
        public Ciudad(string pIDCiuad, string pNombreCiudad, string pNombrePais)
        {
            IDCiudad = pIDCiuad;
            NombreCiudad = pNombreCiudad;
            NombrePais = pNombrePais;

        }
        public Ciudad()
        {
        }
        public void ValidarCiudad()
        {
            if (this.IDCiudad.Trim().Length == 6)
                throw new Exception("El ID ciudad  debe de tener 6 LETRAS");

            if (this.NombreCiudad.Trim().Length <= 1 || NombreCiudad.Trim().Length <= 100)
                throw new Exception("El Nombre de la ciudad admite hasta 100 caracteres ");

            if (this.NombrePais.Trim().Length <= 1 || NombrePais.Trim().Length <= 100)
                throw new Exception("El Nombre del pais admite hasta 100 caracteres ");
        }

    }

}



