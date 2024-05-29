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
    public class CiudadControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AgregarCiudad()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AgregarCiudad(Ciudad C)
        {
            try
            {

                C.ValidarCiudad();

                new Ciudad().AgregarCiudad(C);

                return RedirectToAction("Formulario agregar", "Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }

        }
        [HttpPost]

        [HttpGet]
        public ActionResult BajaCiudad(string IDCiudad)
        {
            try
            {

                 Ciudad _A = new CiudadBD().BajaCiudad(IDCiudad);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe la ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }
        public ActionResult BajaCiudad(Ciudad C)
        {
            try
            {

                new Ciudad().BajaCiudad(C);
                return RedirectToAction("Baja Ciudad", "Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }

        public ActionResult BuscarCiudad(string IDCiudad)
        {
            try
            {
                //obtengo el articulos
                Ciudad _C = new Ciudad().BuscarCiudad(IDCiudad);
                if (_C != null)
                    return View(_C);
                else
                    throw new Exception("No existe la ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }

        [HttpGet]
        public ActionResult ModificarCiudad(string IDCiudad)
        {
            try
            {
                
                Ciudad _C = new Ciudad().BuscarCiudad(IDCiudad);
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
        public ActionResult ModificarCiudad(Ciudad C)
        {
            try
            {
                //valido objeto correcto
                C.ValidarCiudad();

                //intento modificar
                new Ciudad().ModificarCiudad(C);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Ciudad(0, "", 0));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }

        public ActionResult ListarCiudad (string IDCiudad) 
        {
            try
            {
                
                List<Ciudad> _lista = new Ciudad().ListarCiudad();

                 if (_lista.Count >= 1)
                {
                    
                    if (String.IsNullOrEmpty(IDCiudad))
                        return View(_lista); //no hay filtro - muestro compelto
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unA in _lista
                                  where unA.IDCiudad.ToUpper().StartsWith(IDCiudad.ToUpper())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else //no hay datos - no hago nada
                    throw new Exception("No hay Ciudad para mostar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Ciudad>());
            }
        }

    }
}
