using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Persistencia;

namespace SitioMVCAeropuerto.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            Session["Logueo"] = null;
            return View();
        }



        public ActionResult Menu()
        {
            if (Session["Logueo"] is Empleado)
            {
                Session["ListaV"] = null;
                return View();
            }
            else
            {
                return RedirectToAction("FormLogueo", "Empleados");
            }
        }
     }
}
