using EntidadesCompartidas;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
    internal class LogicaEmpleado: ILogicaEmpleado
    {
         //Singleton
        private static LogicaEmpleado _instancia = null;
        private LogicaEmpleado() { }
        public static LogicaEmpleado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaEmpleado();
            return _instancia;
        }

        //operaciones
        public Empleado LogueoEmpleado(string UsuLog , string Contrasena)
        {
            return FabricaPersistencia.GetPersistenciaEmpleado().LogueoEmpleado(UsuLog,Contrasena);
        }

        public Empleado BuscarEmpleado (string UsuLog, Empleado E) // parametro de seguridad
        {
            return (FabricaPersistencia.GetPersistenciaEmpleado().BuscarEmpleado(UsuLog, E  ));  
        }
    }
}
