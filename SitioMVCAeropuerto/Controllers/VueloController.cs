using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EntidadesCompartidas;
using Logica;
using Persistencia;





namespace Sitio.Controllers
{
    public class VuelosControlador : Controller
    {


        [HttpGet]
        public ActionResult FormAltaVuelo()
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logueo"];
                List<Aeropuertos> unaLisA = FabricaLogica.GetLogicaAeropuerto().ListarAeropuerto(_E);
                ViewBag.ListA = new SelectList(unaLisA, "IDAeropuerto", "Nombre");
                ViewBag.ListB = new SelectList(unaLisA, "IDAeropuerto", "Nombre");
                Session["ListaAeropuertos"] = unaLisA;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ListA = new SelectList(null);
                ViewBag.ListB = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormAltaVuelo(Vuelo V)
        {

            try
            {
                Empleado _E = (Empleado)Session["Logueo"];

                if (V.AeropuertoSalida == V.AeropuertoLlegada)
                    throw new Exception("EL AEROPUERTO SALIDA NO PUEDE COINCIDIR CON EL MISMO DE LLEGADA");

                V.AeropuertoSalida = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(V.AeropuertoSalida.IDAeropuerto, _E);
                V.AeropuertoLlegada = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(V.AeropuertoLlegada.IDAeropuerto, _E);
                string codigo = V.AeropuertoSalida.ToString();
                V.IDvuelo = codigo + V.AeropuertoLlegada.IDAeropuerto;
                V.ValidarVuelo();

                Session["Vuelo"] = V;

                FabricaLogica.GetLogicaVuelo().AltaVuelo(V, _E);
                Session["ListaV"] = null;
                return RedirectToAction("FormVueloConsultar", "Vuelos");


            }
            catch (Exception ex)
            {
                List<Aeropuertos> AeropuertosConCP = (List<Aeropuertos>)Session["ListaAeropuertos"];
                ViewBag.ListA = new SelectList(AeropuertosConCP, "IDAeropuerto", "Nombre");
                ViewBag.ListB = new SelectList(AeropuertosConCP, "IDAeropuerto", "Nombre");
                ViewBag.Mensaje = ex.Message;
                return View(new Vuelo());
            }
        }


        public ActionResult FormVueloConsultar()
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Vuelo V = (Vuelo)Session["Vuelo"];

                return View(V);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Vuelo());
            }
        }


        public ActionResult FormListaVuelo(string pFecha, string pOpcion, string pAeropuerto)
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logueo"];

                List<Vuelo> listVuelos = null;

                if (Session["ListaV"] == null)
                {
                    listVuelos = FabricaLogica.GetLogicaVuelo().ListarVuelo(_E);
                    Session["ListaV"] = listVuelos;
                }
                else
                    listVuelos = (List<Vuelo>)Session["ListaVuelo"];

                if (listVuelos.Count == 0)
                    throw new Exception("No hay vuelos Registrados");

                List<Aeropuertos> listA = FabricaLogica.GetLogicaAeropuerto().ListarAeropuerto(_E);
                listA.Insert(0, new Aeropuertos("", "", "Seleccionar", 0, 0, null));
                ViewBag.ListaA = new SelectList(listA, "IDAeropuerto", "Nombre");

                listVuelos = (from unV in listVuelos
                              orderby unV.FechaHoraSalida
                              select unV).ToList();

                if (!String.IsNullOrEmpty(pFecha))
                {
                    listVuelos = (from unV in listVuelos
                                  where unV.FechaHoraSalida.Date == Convert.ToDateTime(pFecha).Date
                                  select unV).ToList();
                }
                if (pOpcion == "Partidas")
                {
                    listVuelos = (from unV in listVuelos
                                  where unV.FechaHoraSalida<= DateTime.Now
                                  select unV).ToList();
                }
                if (pOpcion == "No_Partidas")
                {
                    listVuelos = (from unV in listVuelos
                                  where unV.FechaHoraSalida> DateTime.Now
                                  select unV).ToList();
                }
                if (!String.IsNullOrEmpty(pAeropuerto) && pAeropuerto != "0")
                {
                    listVuelos = (from unV in listVuelos
                                  where unV.AeropuertoSalida.IDAeropuerto == pAeropuerto
                                  select unV).ToList();
                }

                return View(listVuelos);

            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return ViewBag(new List<Vuelo>());
            }
        }

    }
}

