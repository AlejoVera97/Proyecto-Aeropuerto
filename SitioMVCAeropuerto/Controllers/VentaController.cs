using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Logica;




namespace Sitio.Controllers
{
    public class VentaControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //---- ALTA VENTA 

        [HttpGet]
        public ActionResult FormAltaVenta()
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
        public ActionResult FormAltaVenta(Venta V)
        {
            try
            {

                V.ValidarVenta();


            FabricaLogica.GetLogicaVenta().AltaVenta(V);
            return RedirectToAction("Formulario agregar", "Venta");
        }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }


        //---- LISTAR VENTA 
        public ActionResult ListarVenta(int IDVenta)
        {
            try
            {

                List<Venta> _lista = FabricaLogica.GetLogicaVenta().ListarVentas();


                if (_lista.Count >= 1)
                {

                    if (IDVenta == null)
                        return View(_lista);
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unA in _lista
                                  where unA.IDventa.CompareTo().StartsWith(IDVenta.CompareTo())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else //no hay datos - no hago nada
                    throw new Exception("No hay ventas  para mostar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Venta>());
            }
        }


    }
}

