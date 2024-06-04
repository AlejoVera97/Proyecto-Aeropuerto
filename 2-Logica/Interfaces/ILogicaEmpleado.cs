﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntidadesCompartidas;
using Persistencia;
using System.Xml;


namespace Logica
{
    public interface ILogicaEmpleado
    {
        Empleado LogueoEmpleado(string UsuLog, string Contrasena);

        Empleado BuscarEmpleado(string UsuLog, Empleado E);
       
    }

}
