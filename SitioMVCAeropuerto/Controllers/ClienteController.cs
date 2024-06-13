using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Logica;
using Persistencia;






namespace Sitio.Controllers
{
    public class ClienteControlador : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        //------ AGREGAR CLIENTE (LISTO)

        [HttpGet]
        public ActionResult FromAltaCliente()
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
        public ActionResult FormAltaCliente(Clientes C)
        {
            try
            {
                C.ValidarClientes();

                //intento agregar articulo en la bd
                FabricaLogica.GetLogicaCliente().AltaCliente(C, E);
                // no hubo error, alta correcto
                return RedirectToAction("Formulario Alta", "Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }



        //----- MODIFICAR CLIENTE (LISTO)
        [HttpGet]
        public ActionResult FormModificarCliente(string IDPasaporte)
        {
            try
            {

                Clientes _A = FabricaLogica.GetLogicaCliente().BuscarCliente(IDPasaporte, E);
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
        public ActionResult FormModificarCliente(Clientes C)
        {
            try
            {

                C.ValidarClientes();


                FabricaLogica.GetLogicaCliente().ModificarCliente(C, E);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Clientes());
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }



        //--------- BAJA CLIENTE (LISTO)
        [HttpGet]
        public ActionResult FormBajaCliente(string IDPasaporte)
        {
            try
            {
                Clientes _C = FabricaLogica.GetLogicaCliente().BuscarCliente(IDPasaporte, E);
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

        [HttpPost]
        public ActionResult FormBajaCliente(Clientes C)
        {
            try
            {
                //intento eliminar
                FabricaLogica.GetLogicaCliente().BajaCliente(C, E);
                return RedirectToAction("Formulario baja", "Clientes");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }


             
        


        //------- LISTAR CLIENTE (LISTO) 
        public ActionResult FormListarCliente(string IDPasaporte)
        {
            try
            {
                
                List<Clientes> _lista = FabricaLogica.GetLogicaCliente().ListarCliente(e);


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





        //--------- CONSULTA (LISTO)

        public ActionResult FormConsultaCliente (string IDPasaporte)
        {
            try
            {
                //obtengo el 
                Clientes _C = FabricaLogica.GetLogicaCliente().BuscarCliente(IDPasaporte, E);
                if (_C != null)
                    return View(_C);
                else
                    throw new Exception("No existe el Cliente");
            }

            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }
    }
}


