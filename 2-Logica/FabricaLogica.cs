using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    public class FabricaLogica
    {

        public static ILogicaCiudad GetLogicaCiudad()
        {
            return (LogicaCiudad.GetInstancia());

        }

        public static ILogicaCliente GetLogicaCliente()
        {
            return (LogicaCliente.GetInstancia());

        }

        public static ILogicaEmpleado GetLogicaEmpleado()
        {
            return (LogicaEmpleado.GetInstancia());

        }

        public static ILogicaVuelo GetLogicaVuelo()
        {
            return (LogicaVuelo.GetInstancia());
        }

        public static ILogicaVenta GetLogicaVenta()
        {
            return (LogicaVenta.GetInstancia());
        }

        public static ILogicaAeropuerto GetLogicaAeropuerto()
        {
            return (LogicaAeropuerto.GetInstancia());
        }
    }
}



