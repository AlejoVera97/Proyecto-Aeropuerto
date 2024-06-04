using EntidadesCompartidas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
   public interface IPeristenciaEmpleado
    {
        Empleado LogueoEmpleado(string pUsuLog, string pContrasena);

        Empleado BuscarEmpleado(string pUsuLog, Empleado E);
    }
}
