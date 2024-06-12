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
       
        
        // ******************** AGREGAR (LISTO)
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
                E.ValidarEmpleado();

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




        //******************* MODIFICAR (LISTO)

        [HttpGet]
        public ActionResult FormModificarAeropuerto(string IDAeropuerto, Empleado E)
        {
            try
            {
                //obtengo el Aeropeurto
                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto, E);
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
        public ActionResult ModificarAeropuerto(Aeropuertos A,Empleado E)
        {
            try
            {
                //valido objeto correcto
                A.ValidarAeropuerto();
                E.ValidarEmpleado();

                //intento modificar
                FabricaLogica.GetLogicaAeropuerto().ModificarAeropuerto(A,E);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Aeropuertos());
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }





        // ************************ BAJA (LISTO)

        [HttpGet]
        public ActionResult FormBajaAeropuerto(string IDAeropuerto,Empleado E)
        {   
            try
            {
                //obtengo el aeropuertos
                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto,E);
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
        public ActionResult FormBajaAeropuerto(Aeropuertos A, Empleado E)
        {
            try
            {
                //intento eliminar
                 FabricaLogica.GetLogicaAeropuerto().BajaAeropuerto(A,E);
                return RedirectToAction("FormBajaAeropuerto", "Aeropuerto");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Aeropuertos());
            }
        }


        
       

        // ******************** LISTAR (LISTO)
        public ActionResult FormListarAeropuerto(string Nombre,Empleado E )

        {
            try
            {
                //obtengo lista de articulos
                List<Aeropuertos> _lista = FabricaLogica.GetLogicaAeropuerto().ListarAeropuerto(E);

                //si hay datos... defino despliegue
                if (_lista.Count >= 1)
                {
                    //primero reviso si hay que filtrar...
                    if (String.IsNullOrEmpty(Nombre))
                        return View(_lista); //no hay filtro - muestro compelto
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unA in _lista
                                  where unA.Nombre.ToUpper().StartsWith(Nombre.ToUpper())
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




        // ******************  CONSULTAR (LISTO)
        public ActionResult FormAeropuertoConsultar(string IDAeropuerto, Empleado E)
        {
            try
            {
                //obtengo el 
                Aeropuertos _A = FabricaLogica.GetLogicaAeropuerto().BuscarAeropuerto(IDAeropuerto, E);
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


    }
}




