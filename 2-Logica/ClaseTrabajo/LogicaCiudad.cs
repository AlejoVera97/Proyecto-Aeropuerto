using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LogicaCiudad : ILogicaCiudad
    {
        //Singleton
        private static LogicaCiudad _instancia = null;
        private LogicaCiudad() { }
        public static LogicaCiudad GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaCiudad();
            return _instancia;
        }

        //operaciones
        public void AltaCiudad(Ciudad unaCiudad,Empleado E)
        {

            FabricaPersistencia.GetPersistenciaCiudad().AltaCiudad(unaCiudad, E);
        }
        public void BajaCiudad(Ciudad unaCiudad)
        {
            FabricaPersistencia.GetPersistenciaCiudad().BajaCiudad(unaCiudad, E );

        }
        public void ModificarCiudad(Ciudad unaCiudad,Empleado E)
        {
            FabricaPersistencia.GetPersistenciaCiudad().ModificarCiudad(unaCiudad, E);
        }
        public Ciudad BuscarCiudad(string pIDCiudad,Empleado E)
        {
            return (FabricaPersistencia.GetPersistenciaCiudad().BuscarCiudad(pIDCiudad, E ));
        }
        public List<Ciudad> ListarCiudad(Empleado E)
        {
            return (FabricaPersistencia.GetPersistenciaCiudad().ListarCiudad());
        }

    }
 }

