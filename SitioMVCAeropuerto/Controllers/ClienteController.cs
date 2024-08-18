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
        public ActionResult MenuClientes(string Filtro)
        {

            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logueo"];

                List<Clientes> listaClientes = FabricaLogica.GetLogicaCliente().ListarCliente(_E);

                if (listaClientes.Count != 0)
                {

                    if (String.IsNullOrEmpty(Filtro))
                        return View(listaClientes);
                    else
                    {
                        listaClientes = (from unC in listaClientes
                                         where unC.Nombre.ToUpper().StartsWith(Filtro.ToUpper())
                                         select unC).ToList();
                        return View(listaClientes);
                    }
                }
                else
                    throw new Exception("No hay Clientes ingresados en el sistema");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Clientes>());

            }
        }



        [HttpGet]
        public ActionResult FromAltaCliente()
        {
            if (!(Session["Logueo"] is Empleado))
                return RedirectToAction("FormLogueo", "Empleados");
            return View();
        }

        [HttpPost]
        public ActionResult FormAltaCliente(Clientes C)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logeo"];
                C.ValidarClientes();


                FabricaLogica.GetLogicaCliente().AltaCliente(C, _E);
                return RedirectToAction("MeneClientes", "Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }



        [HttpGet]
        public ActionResult FormModificarCliente(string IDPasaporte)
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");
                Empleado _E = (Empleado)Session["Logueo"];

                Clientes C = FabricaLogica.GetLogicaCliente().BuscarCliente(IDPasaporte, _E);
                if (C != null)
                    return View(C);
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
                Empleado _E = (Empleado)Session["Logeo"];

                C.ValidarClientes();
                FabricaLogica.GetLogicaCliente().ModificarCliente(C, _E);
                return RedirectToAction("FormClienteConsultar", "Clientes", new { C.IDPasaporte });
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }



        [HttpGet]
        public ActionResult FormBajaCliente(string IDPasaporte)
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logeo"];
                Clientes _C = FabricaLogica.GetLogicaCliente().BuscarCliente(IDPasaporte, _E);
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
                Empleado _E = (Empleado)Session["Logeo"];

                FabricaLogica.GetLogicaCliente().BajaCliente(C, _E);
                return RedirectToAction("MenuCliente", "Clientes");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Clientes());
            }
        }



        public ActionResult FormConsultaCliente(string IDPasaporte)
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logeo"];
                Clientes _C = FabricaLogica.GetLogicaCliente().BuscarCliente(IDPasaporte, _E);
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


