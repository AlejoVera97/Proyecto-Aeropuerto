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
    public class VentaControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FormularioAltaVenta()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AltaVenta(Venta A)
        {
            try
            {
               
                A.ValidarVenta();

                
                new VentasBD().AltaVenta(A);
                
                return RedirectToAction("Formulario", "Venta");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        public ActionResult ListarVenta(int IDVenta) 
        {
            try
            {
               
                List<Venta> _lista = new VentaBD().ListarVenta();

             
                if (_lista.Count >= 1)
                {
                    
                    if (IDVenta == null)
                        return View(_lista); 
                    else
                    {
                        _lista = (from unA in _lista
                                  where unA.ValidarVenta.ToUpper().StartsWith(IDVenta.ToUpper())
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


