using EntidadesCompartidas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
   public interface IPeristenciaVuelo
    {
        void AltaVuelo(Vuelo pVuelo, Empleado E);

        List<Vuelo> ListarVuelo(Empleado E);

      
    }
}
