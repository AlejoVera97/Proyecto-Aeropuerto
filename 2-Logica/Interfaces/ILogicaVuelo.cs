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
    public interface ILogicaVuelo
    {
        void AltaVuelo(Vuelo vuelo, Empleado E);
        List<Vuelo> ListarVuelo(Empleado E);
    }
}
