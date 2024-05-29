using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Logica;
using Microsoft.AspNetCore.Mvc;
using Persistencia;

//agregar----------------------------
using Sitio.Models;
//------------------------------


namespace Sitio.Controllers
{
    public class AeropuertoControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AgregarAeropuerto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarAeropuerto(Aeropuertos A)
        {
            try
            {
                //valido objeto correcto
                A.ValidarAeropuerto();

                //intento agregar articulo en la bd
                new Aeropuertos().(A);
                // no hubo error, alta correcto
                return RedirectToAction("Formulario agregar", "Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult BajaAeropuerto(Aeropuertos A)
        {
            try
            {
                //intento eliminar
                new FabricaPersistencia().(A);
                return RedirectToAction("Formulario baja Aeropuerto", "Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }
        [HttpGet]
        public ActionResult BajaAeropuerto(string IDAeropuerto)
        {
            try
            {

                Aeropuertos _A = Logica.FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }


        [HttpGet]
        public ActionResult ModificarAeropuerto(string IDAeropuerto)
        {
            try
            {

                Aeropuertos _A = Logica.FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }

        [HttpPost]
        public ActionResult ModificarAeropuerto(Aeropuertos A)
        {
            try
            {
                //valido objeto correcto
                A.ValidarAeropuerto();

                //intento modificar
                new Aeropuertos().ModificarAeropuerto(A);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Aeropuertos(0, "", 0));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }


        public ActionResult BuscarAeropuerto(int IDAeropuerto)
        {
            try
            {
                //obtengo el articulos
                Aeropuertos _A = new Aeropuertos().BuscarAeropuerto(IDAeropuerto);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }


        public ActionResult ListarAeropuerto(int IDAeropuerto) 
        {
            try
            {
                
                List<Aeropuertos> _lista = new Aeropuertos().ListarAeropuerto();

                
                if (_lista.Count >= 1)
                {
                    
                    if (IDAeropuerto == 0)
                            return View(_lista); 
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unA in _lista
                                  where unA.Nombre.ToUpper().StartsWith(IDAeropuerto.ToString())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else //no hay datos - no hago nada
                    throw new Exception("No hay aeropuertos para mostrar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Aeropuertos>());
            }
        }


    }
}


