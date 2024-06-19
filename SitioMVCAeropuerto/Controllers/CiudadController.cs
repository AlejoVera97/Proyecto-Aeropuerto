using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EntidadesCompartidas;
using Logica;
using Persistencia;





namespace Sitio.Controllers
{
    public class CiudadControlador : Controller
    {

        //---------- MENU
        public ActionResult MenuCiudades(string Filtro)
        {


            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logueo"];

                List<Ciudad> listaCiudades = FabricaLogica.GetLogicaCiudad().ListarCiudad(_E);

                if (listaCiudades.Count != 0)
                {

                    if (String.IsNullOrEmpty(Filtro))
                        return View(listaCiudades);
                    else
                    {
                        listaCiudades = (from unaC in listaCiudades
                                         where unaC.NombreCiudad.ToUpper().StartsWith(Filtro.ToUpper())
                                         select unaC).ToList();
                        return View(listaCiudades);
                    }
                }
                else
                    throw new Exception("No hay ciudades ingresadas en el sistema");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Ciudad>());

            }



        }




        // ------------- AGREGAR CIUDAD  
        [HttpGet]
        public ActionResult FormAgregarCiudad()
        {

            if (!(Session["Logueo"] is Empleado))
                return RedirectToAction("FormLogueo", "Empleados");
            return View();
        }

        [HttpPost]
        public ActionResult FormAgregarCiudad(Ciudad C)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logeo"];
              
                if(ModelState.IsValid)
                {
                    C.ValidarCiudad();
                    FabricaLogica.GetLogicaCiudad().AltaCiudad(C, _E);
                    return RedirectToAction("Formulario agregar", "Ciudad");
                }
                return View();             

                
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }



        //----------- BAJA CIUDAD 

        [HttpGet]
        public ActionResult FormBajaCiudad(string IDCiudad)
        
            {
                try
                {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logeo"];

                Ciudad _C = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad, _E);
                    if (_C != null)
                        return View(_C);
                    else
                        throw new Exception("No existe la ciudad");
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View(new Clientes());
                }
            }
       
        public ActionResult FormBajaCiudad(Ciudad C)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logeo"];
                FabricaLogica.GetLogicaCiudad().BajaCiudad(C,_E);
                return RedirectToAction("MenuCiudades", "Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }



      
        //--------- MODIFICAR CIUDAD

        [HttpGet]
        public ActionResult FormModificarCiudad(string IDCiudad)
        {
            try
            {
                if (!(Session["Logeo"] is Empleado))
                    return RedirectToAction("FormLogeo", "Empleado");

                Empleado _E = (Empleado)Session["Logeo"];
                Ciudad _C = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad ,_E);
                if (_C != null)
                    return View(_C);
                else
                    throw new Exception("No existe la Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }

        [HttpPost]
        public ActionResult FormModificarCiudad(Ciudad C)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logeo"];

                if (ModelState.IsValid)
                {

                    C.ValidarCiudad();
                    FabricaLogica.GetLogicaCiudad().ModificarCiudad(C, _E);

                    return RedirectToAction("FormCiudadConsultar", "Ciudad", new { C.IDCiudad });
                }
                return View(C);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }



        //-------- CONSULTAR CIUDAD

        [HttpGet]
        public ActionResult FormCiudadConsultar(string IDCiudad)
        {
            try
            {

                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logueo"];

                Ciudad Ciu = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad, _E);
                if (Ciu != null)
                    return View(Ciu);
                else
                    throw new Exception("No existe la Ciudad");



            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());


            }
        }

    }
}
