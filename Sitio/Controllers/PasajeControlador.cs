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
    public class PasajeControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FormularioAltaPasaje()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AltaPasaje(Pasaje P)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new PasajesBD().AltaPasaje(P);
                    return RedirectToAction("FormlarioAltaPasaje", "Pasaje");
                }
                return View(); 
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        public ActionResult ListarPasaje(int NVenta)
        {
            try
            {

                List<Pasaje> _lista = new PasajeBD().ListarPasaje();


                if (_lista.Count >= 1)
                {

                    if (NVenta == null)
                        return View(_lista);
                    else
                    {

                        _lista = (from unA in _lista
                                  where unA.NAsiento.ToUpper().StartsWith(NVenta.ToUpper())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else 
                    throw new Exception("No hay pasajes para mostar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Clientes>());
            }
        }


    }

}
}
