using EntidadesCompartidas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public interface IPersistenciaCliente
    {
        void AltaCliente(Clientes pCiente, Empleado E);

        void ModificarCliente(Clientes pCiente, Empleado E);

        void BajaCliente(Clientes pCliente, Empleado E);

        Clientes BuscarCliente(string IDPasaporte, Empleado E);
      
        List<Clientes> ListarCliente(Empleado E);
    }
}
