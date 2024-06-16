using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Logica;
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
            //----- LOGUEO
            [HttpGet]
            public ActionResult FormLogueo()
            {
                return View();
            }

            [HttpPost]
            public ActionResult FormLogueo(string usuLog,string contranesa)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        FabricaLogica.GetLogicaEmpleado().LogueoEmpleado(usuLog,contranesa);

                        Session["Logueo"] = usuLog;
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


            //----- DESLOGEO

            public ActionResult Deslogueo()
            {
                return RedirectToAction("Index", "Home");
            }
        }


    }
}












