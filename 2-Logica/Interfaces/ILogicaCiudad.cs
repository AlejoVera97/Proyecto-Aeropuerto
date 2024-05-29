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
   public interface ILogicaCiudad
    {
       void AltaCiudad(Ciudad ciudad, Empleado E);

        void BajaCiudad(Ciudad ciudad);

        void ModificarCiudad(Ciudad ciudad, Empleado E);

        Ciudad BuscarCiudad(string IDCiudad , Empleado E);
        List<Ciudad> ListarCiudad(Empleado E);
     
    }
}
