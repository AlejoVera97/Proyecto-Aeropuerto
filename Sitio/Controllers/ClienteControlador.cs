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
    public class ClienteControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AltaCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AltaCliente(Clientes C)
        {
            try
            {
                      C.ValidarClientes();

                
                new Aeropuertos().AltaCliente(C);
                
                return RedirectToAction("Formulario Alta", "Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult ModificarCliente(string  IDPasaporte)
        {
            try
            {
                
                Clientes _A = new ClientesBD().BuscarCliente(IDPasaporte);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }

        [HttpPost]
        public ActionResult ModificarCliente(Clientes C)
        {
            try
            {
                
                C.ValidarClientes();

                
                new ClientesBD().ModificarCliente(C);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Clientes (0, "", 0));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }

        [HttpGet]
        public ActionResult BajaCliente(string IDPasaporte)
        {
            try
            {
               
                Clientes _A = new ClientesBD().BuscarCliente(IDPasaporte);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }

        [HttpPost]
        public ActionResult BajaCliente(Clientes A)
        {
            try
            {
                //intento eliminar
                new ClientesBD().BajaCliente(A);
                return RedirectToAction("Formulario baja", "Clientes");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }


        public ActionResult BuscarCliente(string IDpasaporte)
        {
            try
            {
                //obtengo el articulos
                Clientes _C = new Clientes().BuscarClientes.(IDpasaporte);
                if (_C != null)
                    return View(_C);
                else
                    throw new Exception("No existe el cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }


        public ActionResult ListarCliente(string IDPasaporte)
        { 
            try
            {
                
                List<Clientes> _lista = new ClientesBD().ListarCliente();

                
                if (_lista.Count >= 1)
                {
                    
                    if (String.IsNullOrEmpty(IDPasaporte))
                        return View(_lista);
                    else
                    {
                        
                        _lista = (from unA in _lista
                                  where unA.IDPasaporte.ToUpper().StartsWith(IDPasaporte.ToUpper())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else //no hay datos - no hago nada
                    throw new Exception("No hay Clientes para mostar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Clientes>());
            }
        }


    }
}


