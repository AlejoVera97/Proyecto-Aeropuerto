using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public class FabricaPersistencia
    {
        public static IPersistenciaAeropuerto GetPersistenciaAeropuerto()
        {
            return (PersistenciaAeropuerto.GetInstancia());
        }

        public static IPersistenciaCiudad GetPersistenciaCiudad()
        {
            return (PersistenciaCiudad.GetInstancia());
        }

        public static IPersistenciaCliente GetPersistenciaCliente()
        {
            return (PersistenciaCliente.GetInstancia());
        }

        public static IPeristenciaEmpleado GetPersistenciaEmpleado()
        {
            return (PersistenciaEmpleado.GetInstancia());
        }   

        public static IPersistenciaVenta GetPersistenciaVenta()
        {
            return (PersitenciaVenta.GetInstancia());
        }

        public static IPeristenciaVuelo GetPersistenciaVuelo()
        {
            return (PersitenciaVuelo.GetInstancia());
        }
    }
}
