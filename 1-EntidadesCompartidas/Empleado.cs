using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EntidadesCompartidas
{
    public class Empleado
    {
        //atributos
        private string _UsuLog;
        private string _NombreCompleto;
        private string _Contrasena;
        private string _Labor;

        //Propiedades
        public string UsuLog
        {
            get { return _UsuLog; }
            set { _UsuLog = value; }
        }

        public string NombreCompleto
        {
            get { return _NombreCompleto; }
            set { _NombreCompleto = value; }

        }

        public string Contrasena
        {
            get { return _Contrasena; }
            set { _Contrasena = value; }

        }

        public string Labor
        {
            get { return _Labor; }
            set { _Labor = value; }

        }


        public Empleado(string pUsuLog, string pNombreCompleto, string pContrasena, string pLabor)
        {

            UsuLog = pUsuLog;
            NombreCompleto = pNombreCompleto;
            Contrasena = pContrasena;
            Labor = pLabor;
        }

        public Empleado()
        {

        }

        public void ValidarEmpleado()
        {
            if (this.UsuLog.Trim().Length != 8)
                throw new Exception("El Usuario debe de tener 8 caracteres");
            if (this.NombreCompleto.Trim().Length <= 1 || NombreCompleto.Trim().Length <= 100)
                throw new Exception("El Nombre completo debe estar comprendido entre 1 - 100");
            if (this.Contrasena.Trim().Length <= 6)
                throw new Exception(" La contrasena debe de ser de 6 caracteres");
            if (this.Labor != "empleado" && this.Labor != "admin" && this.Labor != "gerente")
                throw new Exception("La opción para el campo Labor solo puede ser 'empleado', 'admin' o 'gerente'.");
        }

    }
}

    

