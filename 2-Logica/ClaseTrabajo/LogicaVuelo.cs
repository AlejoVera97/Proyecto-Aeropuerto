using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using Persistencia;
using System.Xml;

namespace Logica
{
    internal class LogicaVuelo : ILogicaVuelo

    {

        private static LogicaVuelo _instancia = null;

        private LogicaVuelo() { }

        public static LogicaVuelo GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaVuelo();
            return _instancia;
        }

        public void AltaVuelo(Vuelo V,Empleado E)
        {
            //falta validar que la fecha de salida del vuelo sea a futuro
            FabricaPersistencia.GetPersistenciaVuelo().AltaVuelo(V, E);
        }

        public List<Vuelo> ListarVuelo(Empleado E)
        {
            return (FabricaPersistencia.GetPersistenciaVuelo().ListarVuelo(E));
        }
    }
}
