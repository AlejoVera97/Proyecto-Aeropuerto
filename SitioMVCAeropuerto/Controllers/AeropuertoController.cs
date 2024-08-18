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
    public class AeropuertoControlador : Controller
    {


        public ActionResult MenuAeropuertos(string Filtro)
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado logueado = (Empleado)Session["Logueo"];

                List<Aeropuertos> listAeropuertos = FabricaLogica.GetLogicaAeropuerto().ListarAeropuerto(logueado);

                if (listAeropuertos.Count != 0)
                {

                    if (String.IsNullOrEmpty(Filtro))
                        return View(listAeropuertos);
                    else
                    {
                        listAeropuertos = (from unA in listAeropuertos
                                           where unA.Nombre.ToUpper().StartsWith(Filtro.ToUpper())
                                           select unA).ToList();
                        return View(listAeropuertos);
                    }
                }
                else
                    throw new Exception("No hay Aeropuertos ingresados en el sistema");

            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Aeropuertos>());
            }
        }


        [HttpGet]
        public ActionResult FormAgregarAeropuerto()
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado logueado = (Empleado)Session["Logueo"];
                List<Ciudad> unaLisC = FabricaLogica.GetLogicaCiudad().ListarCiudad(logueado);
                ViewBag.ListaC = new SelectList(unaLisC, "codCiudad", "ciu_Nombre");
                Session["ListaCiudades"] = unaLisC;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ListaC = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormAgregarAeropuerto(Aeropuertos A)
        {

            try
            {
                Empleado logueado = (Empleado)Session["Logueo"];
                A._Ciudad = FabricaLogica.GetLogicaCiudad().BuscarCiudad(A._Ciudad.IDCiudad, logueado);

                if (!(ModelState.IsValid))
                    throw new Exception("Complete los datos requeridos");

                A.ValidarAeropuerto();
                FabricaLogica.GetLogicaAeropuerto().AltaAeropuerto(A, logueado);
                return RedirectToAction("MenuAeropuertos", "Aeropuertos");


            }
            catch (Exception ex)
            {
                List<Ciudad> ciudadPais = (List<Ciudad>)Session["ListaCiudades"];
                ViewBag.ListaC = new SelectList(ciudadPais, "IDCiudad", "NombreCiudad");
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }



        [HttpGet]
        public ActionResult FormModificarAeropuerto(string IDAeropuerto)
        {
            try
            {
                if (!(Session["Logeo"] is Empleado))
                    return RedirectToAction("FormLogeo", "Empleado");

                Empleado _E = (Empleado)Session["Logueo"];

                List<Ciudad> unListaCiudad = FabricaLogica.GetLogicaCiudad().ListarCiudad(_E);
                Session["ListarCiudades"] = unListaCiudad;

                ViewBag.unListaCiudad = new SelectList(unListaCiudad, "IDCiudad", "CiudadNombre");

                Aeropuertos unA = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto, _E);

                if (unA != null)
                    return View(unA);
                else
                    throw new Exception("No existe el Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.unListaCiudad = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }

        [HttpPost]
        public ActionResult ModificarAeropuerto(Aeropuertos A)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logueo"];

                A._Ciudad = FabricaLogica.GetLogicaCiudad().BuscarCiudad(A._Ciudad.IDCiudad, _E);
                A.ValidarAeropuerto();


                FabricaLogica.GetLogicaAeropuerto().ModificarAeropuerto(A, _E);
                return RedirectToAction("FormAeropuertoConsulta", "Aeropuertos", new { A.IDAeropuerto });
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                List<Ciudad> _CiudadPais = (List<Ciudad>)Session["ListaCiudad"];
                ViewBag.ListaCiudad = new SelectList(_CiudadPais, "IDCiudad", "CiudadNombre");
                return View(new Aeropuertos());
            }
        }



        [HttpGet]
        public ActionResult FormBajaAeropuerto(string IDAeropuerto)
        {
            try
            {
                if (!(Session["Logeo"] is Empleado))
                    return RedirectToAction("Logeo", "Empleado");

                Empleado _E = (Empleado)Session["Logeo"];

                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto, _E);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el AEROPUERTO");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }

        [HttpPost]
        public ActionResult FormBajaAeropuerto(Aeropuertos A)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logeo"];
                FabricaLogica.GetLogicaAeropuerto().BajaAeropuerto(A, _E);
                return RedirectToAction("MenuAeropuerto", "Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }



        public ActionResult FormAeropuertoConsultar(string IDAeropuerto)
        {
            try
            {
                if (!(Session["Logeo"] is Empleado))
                    return RedirectToAction("FormLogeo", "Empleados");


                Empleado _E = (Empleado)Session["Logeo"];

                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto, _E);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el aeropuerto");
            }

            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }


    }
}




