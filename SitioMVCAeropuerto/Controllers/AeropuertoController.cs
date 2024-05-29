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
    public class AeropuertoControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
       
        
        // ******************** AGREGAR
        [HttpGet]
        public ActionResult FormAgregarAeropuerto()
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
        public ActionResult FormAgregarAeropuerto(Aeropuertos A, Empleado E)
        {
            try
            {
                //valido objeto correcto
                A.ValidarAeropuerto();

                //intento agregar articulo en la bd
                FabricaLogica.GetLogicaAeropuerto().AltaAeropuerto(A, E);
                // no hubo error, alta correcto
                return RedirectToAction("Formulario agregar", "Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }


        //******************* MODIFICAR 

        [HttpGet]
        public ActionResult FormModificarAeropuerto(Aeropuertos A, Empleado E)
        {
            try
            {
                //obtengo el Aeropeurto
                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().ModificarAeropuerto(A,E);
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
        public ActionResult FormModificarAeropuerto(Aeropuertos A, Empleado E)
        {
            try
            {
                //valido objeto correcto
                A.ValidarAeropuerto();

                //intento modificar
                FabricaLogica.GetLogicaAeropuerto().ModificarAeropuerto(A, E);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Aeropuertos());
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }





        // ************************ BAJA

        [HttpGet]
        public ActionResult FormBajaAeropuerto(string IDAeropuerto)
        {   
            try
            {
                //obtengo el aeropuertos
                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el AEROPUERTO");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }

        [HttpPost]
        public ActionResult FormBajaAeropuerto(Aeropuertos A)
        {
            try
            {
                //intento eliminar
                 FabricaLogica.GetLogicaAeropuerto().BajaAeropuerto(A);
                return RedirectToAction("FormBajaAeropuerto", "Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }


        // ******************* BUSCAR

        public ActionResult BuscarAeropuerto(string IDAeropuerto)
        {
            try
            {
                //obtengo el articulos
                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto); ;
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

        // ******************** LISTA 
        public ActionResult FormListarAeropuerto(Empleado E)

        {
            try
            {
                //obtengo lista de articulos
                List<Aeropuertos> _lista = FabricaLogica.GetLogicaAeropuerto().ListarAeropuerto(E);

                //si hay datos... defino despliegue
                if (_lista.Count >= 1)
                {
                    //primero reviso si hay que filtrar...
                    if (String.IsNullOrEmpty(IDAeropuerto))
                        return View(_lista); //no hay filtro - muestro compelto
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unA in _lista
                                  where unA.Nombre.ToUpper().StartsWith(IDAeropuerto.ToUpper())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else //no hay datos - no hago nada
                    throw new Exception("No hay Articulos para mostar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Aeropuertos>());
            }
        }





    }
}




