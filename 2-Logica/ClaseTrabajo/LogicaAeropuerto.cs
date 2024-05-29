using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using Persistencia;


namespace Logica
{
    internal class LogicaAeropuerto : ILogicaAeropuerto
    {

        //Singleton

        private static LogicaAeropuerto _instancia = null;
        private LogicaAeropuerto() { }
        public static LogicaAeropuerto GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaAeropuerto();
            return _instancia;
        }

        // PARAMETRO DE SEGURIDAD CASO 4D PRIMER DISENO 1
        //operaciones
        public void AltaAeropuerto(Aeropuertos A,Empleado E)
        {
            FabricaPersistencia.GetPersistenciaAeropuerto().AltaAeropuerto(A, E);

        }
        public void BajaAeropuerto(Aeropuertos A)
        {
            FabricaPersistencia.GetPersistenciaAeropuerto().BajaAeropuerto(A);

        }
        public void ModificarAeropuerto(Aeropuertos A,Empleado E)
        {
            FabricaPersistencia.GetPersistenciaAeropuerto().ModificarAeropuerto(A, E );
        }
        public Aeropuertos BuscarAeropuerto(string pIDAeropuerto,Empleado E)
        {
            return (FabricaPersistencia.GetPersistenciaAeropuerto().BuscarAeropuerto(pIDAeropuerto, E));
        }
        public List<Aeropuertos> ListarAeropuerto(Empleado E)
        {
            return (FabricaPersistencia.GetPersistenciaAeropuerto().ListarAeropuerto());
        }

    }
}

