using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Persistencia
{
   public interface IPersistenciaCiudad
    {
        void AltaCiudad(Ciudad unaCiudad, Empleado E);
        void BajaCiudad(Ciudad unaCiudad, Empleado E);

        void ModificarCiudad(Ciudad unaCiudad, Empleado E);

        Ciudad BuscarCiudad(string IDCiudad, Empleado E );

        List<Ciudad> ListarCiudad();
    }
}

