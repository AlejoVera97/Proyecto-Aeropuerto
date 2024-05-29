using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using Persistencia;
using System.Xml;

namespace Logica
{
    internal class LogicaVenta :ILogicaVenta 
    {
        
        private static LogicaVenta _instancia = null;
        private LogicaVenta() { }
        public static LogicaVenta GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaVenta();
            return _instancia;
        }

        //operaciones

        public void AltaVenta(Venta unVenta, Empleado E)
        {
            FabricaPersistencia.GetPersistenciaVenta().AltaVenta(unVenta, E );
        }

        public List<Venta> ListarVentas(Vuelo unVuelo, Empleado E ) 
        {
            return (FabricaPersistencia.GetPersistenciaVenta().ListarVentas(unVuelo,E));
        }


    }
}
