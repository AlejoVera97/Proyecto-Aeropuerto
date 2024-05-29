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
        public ActionResult Index()
        {
            return View();
        }


        //----- ALTA VUELO
        [HttpGet]
        public ActionResult FormAltaVuelo()
        {
            try
            {
                //muestro la vista
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormAltaVuelo(Vuelo V)
        {
            try
            {
                //valido objeto correcto
                V.ValidarVuelo();

                //intento agregar articulo en la bd
                FabricaLogica.GetLogicaVuelo().AltaVuelo(V);
                // no hubo error, alta correcto
                return RedirectToAction("Formulario agregar", "Vuelo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }


        //----- LISTADO DE VUELOS
        public ActionResult FormListarVuelo(string IDVuelo)
        {
            try
            {

                List<Vuelo> _lista = FabricaLogica.GetLogicaVuelo().ListarVuelo();
                if (_lista.Count >= 1)
                {

                    if (String.IsNullOrEmpty(IDVuelo))
                        return View(_lista); //no hay filtro - muestro compelto
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unA in _lista
                                  where unA.IDvuelo.ToUpper().StartsWith(IDVuelo.ToUpper())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else //no hay datos - no hago nada
                    throw new Exception("No hay Vuelos para mostar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Ciudad>());
            }
        }
    }
}
