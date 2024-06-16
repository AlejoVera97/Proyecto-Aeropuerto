using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntidadesCompartidas;
using Persistencia;
using System.Xml;

namespace Logica
{
   public  interface ILogicaCliente
    {
        void AltaCliente(Clientes unCliente, Empleado E);

        void BajaCliente(Clientes unCliente, Empleado E);

        void ModificarCliente(Clientes unCliente, Empleado E);

        Clientes BuscarCliente(string IDPasaporte, Empleado E);

        List<Clientes> ListarCliente(Empleado E);
      
  }
}
