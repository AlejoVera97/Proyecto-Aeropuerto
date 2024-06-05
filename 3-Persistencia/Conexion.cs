using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{

    internal class Conexion
    {
        internal static string Cnn(EntidadesCompartidas.Empleado UsuLog = null)
        {
            if (UsuLog == null)
                return "Data Source =.; Initial Catalog = AeropuertosAmericanos; Integrated Security = true";
            else
                return "Data Source =.; Initial Catalog = AeropuertosAmericanos; Empleado =" + UsuLog.UsuLog + "; Password='" + UsuLog.Contrasena + "'";
        }
    }
}
