using EntidadesCompartidas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public interface IPersistenciaAeropuerto
    {
        void AltaAeropuerto(Aeropuertos A, Empleado E);

        void BajaAeropuerto(Aeropuertos A, Empleado E);

        void ModificarAeropuerto(Aeropuertos A, Empleado E);

        Aeropuertos BuscarAeropuerto(string pIDAeropuerto, Empleado E);

        List<Aeropuertos> ListarAeropuerto(Empleado E);
    }
}