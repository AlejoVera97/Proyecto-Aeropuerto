using EntidadesCompartidas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
   public interface IPersistenciaVenta
    {
        void AltaVenta(Venta pVenta, Empleado E);

        List<Venta> ListarVentas( Vuelo V, Empleado E);

      
    }
}
