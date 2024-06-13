using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EntidadesCompartidas;
using Logica;
using Persistencia;





namespace Sitio.Controllers
{
    public class CiudadControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        // ------------- AGREGAR CIUDAD  (LISTO)
        [HttpGet]
        public ActionResult FormAgregarCiudad()
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
        public ActionResult FormAgregarCiudad(Ciudad C)
        {
            try
            {
                //valido objeto correcto
                C.ValidarCiudad();
                E.ValidarEmpleado();

                //intento agregar articulo en la bd
                FabricaLogica.GetLogicaCiudad().AltaCiudad(C,E);
                // no hubo error, alta correcto
                return RedirectToAction("Formulario agregar", "Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }



        //----------- BAJA CIUDAD (LISTO)

        [HttpGet]
        public ActionResult FormBajaCiudad(string IDCiudad)
        
            {
                try
                {
                    Ciudad _C = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad, E);
                    if (_C != null)
                        return View(_C);
                    else
                        throw new Exception("No existe el CLIENTE");
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View(new Clientes());
                }
            }
       
        public ActionResult FormBajaCiudad(Ciudad C)
        {
            try
            {

                FabricaLogica.GetLogicaCiudad().BajaCiudad(C,E);
                return RedirectToAction("Baja Ciudad", "Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }



        //---------- BUSCAR CIUDAD (LISTO)
        public ActionResult FormBuscarCiudad(string IDCiudad)
        {
            try
            {

                
                Ciudad _C = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad,E);
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




        //--------- MODIFICAR CIUDAD

        [HttpGet]
        public ActionResult FormModificarCiudad(string IDCiudad)
        {
            try
            {
                Ciudad _C = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad ,E);
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
        public ActionResult FormModificarCiudad(Ciudad C)
        {
            try
            {
                //valido objeto correcto
                C.ValidarCiudad();

                //intento modificar
                FabricaLogica.GetLogicaCiudad().ModificarCiudad(C, E);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Ciudad());
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }



        //-------- LISTAR CIUDAD

        public ActionResult FormListarCiudad(string IDCiudad)
        {
            try
            {
                //obtengo lista de articulos
                List<Ciudad> _lista = FabricaLogica.GetLogicaCiudad().ListarCiudad(E);

                //si hay datos... defino despliegue
                if (_lista.Count >= 1)
                {
                    //primero reviso si hay que filtrar...
                    if (String.IsNullOrEmpty(IDCiudad))
                        return View(_lista); //no hay filtro - muestro compelto
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unC in _lista
                                  where unC.IDCiudad.ToUpper().StartsWith(IDCiudad.ToUpper())
                                  select unC).ToList();
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
