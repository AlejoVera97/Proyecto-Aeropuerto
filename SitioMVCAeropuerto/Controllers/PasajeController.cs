using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Logica;



namespace Sitio.Controllers
{
    public class PasajeControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        // -----ALTA PASAJE 
        [HttpGet]
        public ActionResult FormAltaPasaje()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormAltaPasaje(Pasaje P)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FabricaLogica.GetLogicaVenta().AltaVenta(P);
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



        //----- LISTAR PASAJES 

        public ActionResult FormListarPasaje(int NVenta)
        {
            try
            {

                List<Pasaje> _lista = FabricaLogica.GetLogicaVenta().ListarVentas(Pasaje);


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
}
