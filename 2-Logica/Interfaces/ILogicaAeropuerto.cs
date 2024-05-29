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
    public interface ILogicaAeropuerto
    {
        void AltaAeropuerto(Aeropuertos A,Empleado E);
        void BajaAeropuerto(Aeropuertos A);
        void ModificarAeropuerto(Aeropuertos A,Empleado E);
        Aeropuertos BuscarAeropuerto(string pIDAeropuerto, Empleado E);
        List<Aeropuertos> ListarAeropuerto(Empleado E);
    }
}




