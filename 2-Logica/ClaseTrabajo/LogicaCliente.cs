using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LogicaCliente : ILogicaCliente
    {
        //Singleton
        private static LogicaCliente _instancia = null;
        private LogicaCliente() { }
        public static LogicaCliente GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaCliente();
            return _instancia;
        }

        //operaciones
        public void AltaCliente(Clientes unCliente,Empleado E)
        {
            FabricaPersistencia.GetPersistenciaCliente().AltaCliente(unCliente, E   );
        }
        public void BajaCliente(Clientes unCliente, Empleado E)
        {
            FabricaPersistencia.GetPersistenciaCliente().BajaCliente(unCliente, E);
        }
        public void ModificarCliente(Clientes unCliente,Empleado E)
        {
            FabricaPersistencia.GetPersistenciaCliente().ModificarCliente(unCliente, E);
        }
        public Clientes BuscarCliente(string pIDPasaporte,Empleado E)
        {
            return FabricaPersistencia.GetPersistenciaCliente().BuscarCliente(pIDPasaporte, E   );
        }
        public List<Clientes> ListarCliente(Empleado E)
        {
            return FabricaPersistencia.GetPersistenciaCliente().ListarCliente(E);
        }

    }
}


