using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{

    internal class Conexion
    {
        internal static string Cnn(EntidadesCompartidas.Empleado pUsuLog = null)
        {
            if (pUsuLog == null)
                return "Data Source =.; Initial Catalog = EjemploBD; Integrated Security = true";
            else
                return "Data Source =.; Initial Catalog = EjemploBD; User=" + pUsuLog + "; Password='" + pUsuLog.Contrasena + "'";
        }
    }
}
