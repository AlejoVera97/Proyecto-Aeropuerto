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


        // ------------- AGREGAR CIUDAD
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

                //intento agregar articulo en la bd
                FabricaLogica.GetLogicaCiudad().AltaCiudad(C);
                // no hubo error, alta correcto
                return RedirectToAction("Formulario agregar", "Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }



        //----------- BAJA CIUDAD

        [HttpGet]
        public ActionResult FormBajaCiudad(string IDCiudad)
        {
            try
            {

                Ciudad _C =  FabricaLogica.GetLogicaCiudad().BajaCiudad(IDCiudad);
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
        public ActionResult FormBajaCiudad(Ciudad C)
        {
            try
            {

                FabricaLogica.GetLogicaCiudad().BajaCiudad(C);
                return RedirectToAction("Baja Ciudad", "Ciudad");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Ciudad());
            }
        }



        //---------- BUSCAR CIUDAD
        public ActionResult FormBuscarCiudad(string IDCiudad)
        {
            try
            {

                //obtengo el articulos
                Ciudad _C = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad);
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
                Ciudad _C = FabricaLogica.GetLogicaCiudad().BuscarCiudad(IDCiudad);
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
                FabricaLogica.GetLogicaCiudad().ModificarCiudad(C);
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

                List<Ciudad> _lista = FabricaLogica.GetLogicaCiudad().ListarCiudad();
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
