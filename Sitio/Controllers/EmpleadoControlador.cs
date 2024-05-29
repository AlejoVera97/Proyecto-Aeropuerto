using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Microsoft.AspNetCore.Mvc;


//agregar----------------------------
using Sitio.Models;
//------------------------------


namespace Sitio.Controllers
{
    public class EmpleadoControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public class UsuariosController : Controller
        {
           
            [HttpGet]
            public ActionResult FormularioLogueo()
            {
                return View();
            }

            [HttpPost]
            public ActionResult FormularioLogueo(Empleado E             )
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        new EmpleadoBD().LogueoEmpleado(E);
                       
                        Session["Logueo"] = E;
                        return RedirectToAction("Principal", "Home");
                    }
                    else
                        return View();

                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View();
                }
            }


     
            public ActionResult Deslogueo()
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult BuscarEmpleado(string UsuLog)
        {
            try
            {
                //obtengo el articulos
                Empleado _E = new Empleado().BuscarEmpleado.(UsuLog);
                if (_E != null)
                    return View(_E);
                else
                    throw new Exception("No existe el empleado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Empleado());
            }
        }
    }
}












