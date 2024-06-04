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
   public interface ILogicaVenta
    {
        void AltaVenta(Venta unaV, Empleado E  );

        List<Venta> ListarVentas  (Vuelo V, Empleado E);

    }
}
