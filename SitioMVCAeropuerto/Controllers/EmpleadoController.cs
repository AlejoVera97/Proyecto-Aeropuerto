using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Logica;





namespace Sitio.Controllers
{
    public class EmpleadoControlador : Controller
    {

        public class UsuariosController : Controller
        {
            
            [HttpGet]
            public ActionResult FormLogueo()
            {
                return View();
            }

            [HttpPost]
            public ActionResult FormLogueo(Empleado E)
            {
                try
                {
                    if (E.Contrasena == null || E.UsuLog == null)
                        throw new Exception("no puede dejar ningún campos vacios");
                    E = FabricaLogica.GetLogicaEmpleado().LogueoEmpleado(E.UsuLog, E.Contrasena);
                    if (E == null)
                    {
                        ViewBag.Mensaje = "Usuario / Contraseña Incorrectos";
                        return View();
                    }
                    Session["Logueo"] = E;
                    return RedirectToAction("Menu", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View();
                }

            }

        }

    }
}












