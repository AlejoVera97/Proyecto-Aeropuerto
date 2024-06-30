using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesCompartidas;
using Logica;




namespace Sitio.Controllers
{
    public class VentaControlador : Controller
    {
        [HttpGet]
        public ActionResult FormAltaVenta()
        {

            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logueo"];
                List<Vuelo> unaListaV = FabricaLogica.GetLogicaVuelo().ListarVuelo(_E);
                unaListaV = (from unV in unaListaV
                             where unV.FechaHoraSalida > DateTime.Now
                             select unV).ToList();

                List<Clientes> unaListaC = FabricaLogica.GetLogicaCliente().ListarCliente(_E);

                ViewBag.ListaV = new SelectList(unaListaV, "CodVuelo", "CodVuelo");
                ViewBag.ListaC = new SelectList(unaListaC, "NumPasaporte", "NomCli");
                Session["ListaVuelos"] = unaListaV;
                Session["ListaClientes"] = unaListaC;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ListaV = new SelectList(null);
                ViewBag.ListaC = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormAltaVenta(Venta V)
        {
            try
            {
                V.Empleado = (Empleado)Session["Logueo"];

                List<Vuelo> unaListV = (List<Vuelo>)Session["ListaVuelos"];

                V.Vuelo = unaListV.Where(Vuelo => Vuelo.IDvuelo == Vuelo.IDvuelo).FirstOrDefault();

                List<Clientes> unaListaC = (List<Clientes>)Session["ListaClientes"];

                V.Clientes = unaListaC.Where(Clientes => Clientes.IDPasaporte == V.Clientes.IDPasaporte).FirstOrDefault();

                List<Venta> listaVenta = FabricaLogica.GetLogicaVenta().ListarVentas(V.Vuelo, V.Empleado);

                int cantAsientos = (from unA in listaVenta
                                    from unP in unA.VentaLista
                                    select unP.NAsiento).Count();

                if (V.Vuelo.CantidadAsientos < cantAsientos)
                    throw new Exception("El vuelo esta completo");


                Session["Disponibles"] = (V.Vuelo.CantidadAsientos - cantAsientos);

                V.VentaLista = new List<Pasaje>();

                Session["ListaDeVentas"] = listaVenta;
                Session["Venta"] = V;

                return RedirectToAction("FormAltaPasaje", "Ventas");

            }
            catch (Exception ex)
            {
                List<Clientes> unaLisC = (List<Clientes>)Session["ListaClientes"];
                List<Vuelo> unaLisV = (List<Vuelo>)Session["ListaVuelos"];

                ViewBag.ListV = new SelectList(unaLisV, "CodVuelo", "CodVuelo");
                ViewBag.ListC = new SelectList(unaLisC, "NumPasaporte", "NomCli");

                ViewBag.Mensaje = ex.Message;
                return View(new Venta());
            }

        }

        [HttpGet]
        public ActionResult FormAltaPasaje()
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Empleado _E = (Empleado)Session["Logueo"];

                List<Clientes> unaLisC = (List<Clientes>)Session["ListaClientes"];

                ViewBag.Disponibles = Session["Disponibles"].ToString();

                ViewBag.unaLisC = new SelectList(unaLisC, "IDPasaporte", "Nombre");

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.unaLisC = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormAltaPasaje(Pasaje unPasaje)
        {
            try
            {

                Empleado _E = (Empleado)Session["Logueo"];
                Venta venta = (Venta)Session["Venta"];

                unPasaje.IDPasaporte = (from IDPasaporte in (List<Clientes>)Session["ListaClientes"]
                                        where IDPasaporte == unPasaje.IDPasaporte
                                        select IDPasaporte).First();

                if (unPasaje.NAsiento > venta.Vuelo.CantidadAsientos)
                    throw new Exception("ERRPR -EL NUMERO DE ASIENTO ES MAYOR A LA CANTIDAD DE ASIENTO QUE POSEE EL VUELO ");

                unPasaje.ValidarPasaje();

                bool asientoV = (from unA in ((Venta)Session["Venta"]).VentaLista
                                 where unA.NAsiento == unPasaje.NAsiento
                                 select unA).Any();
                if (asientoV)
                    throw new Exception("ERROR - YA SE VENDIO EL ASIENTO");

                bool clienteV = (from unA in ((Venta)Session["Venta"]).VentaLista
                                 where unA.IDPasaporte == unPasaje.IDPasaporte
                                 select unA).Any();
                if (clienteV)
                    throw new Exception("ERROR -  YA COMPRO UN ASIENTO ESE CLIENTE");

                bool repiteAsiento = (from unaV in ((List<Venta>)Session["ListaDeVentas"])
                                      from unP in unaV.VentaLista
                                      where unP.NAsiento == unPasaje.NAsiento
                                      select unP).Any();

                if (repiteAsiento)
                    throw new Exception("ERROR- EL ASIENTO YA FUE VENDIDO ");

                bool repiteCliente = (from unA in ((List<Venta>)Session["ListaDeVentas"])
                                      from unP in unA.VentaLista
                                      where unP.IDPasaporte == unPasaje.IDPasaporte
                                      select unP).Any();

                if (repiteCliente)
                    throw new Exception("ERROR - EL CLIENTE YA TIENE UN ASIENTO EN ESE VUELO");


                ((Venta)Session["Venta"]).VentaLista.Add(unPasaje);

                Session["Disponibles"] = ((int)Session["Disponibles"] - 1);

                ViewBag.Disponibles = Session["Disponibles"].ToString();

                return RedirectToAction("FormAltaPasaje", "Ventas");
            }
            catch (Exception ex)
            {
                ViewBag.Disponibles = Session["Disponibles"].ToString();
                List<Clientes> unaLisCP = (List<Clientes>)Session["ListaClientes"];
                ViewBag.unaLisC = new SelectList(unaLisCP, "NumPasaporte", "NomCli");
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        public ActionResult GuardarVenta()
        {
            try
            {
                Empleado _E = (Empleado)Session["Logueo"];

                Venta unaV = (Venta)Session["Venta"];

                int _ImpuestoPartida = unaV.Vuelo.AeropuertoSalida.ImpuestoParitda;
                int _ImpuestoLlegada = unaV.Vuelo.AeropuertoLlegada.ImpuestoLlegada;
                double _Precio = unaV.Vuelo.PrecioVuelo;
                int _Cantidad = unaV.VentaLista.Count();
                double _PrecioFinal = (_ImpuestoPartida + _ImpuestoLlegada + _Precio) * _Cantidad;
                unaV.Precio = _PrecioFinal;

                unaV.ValidarVenta();

                FabricaLogica.GetLogicaVenta().AltaVenta(unaV, _E);

                return RedirectToAction("MostrarVenta", "Ventas");
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.Message;
                return RedirectToAction("AltaFallo", "Ventas");
            }
        }

        public ActionResult MostrarVenta()
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                Venta unaV = (Venta)Session["Venta"];

                return View(unaV);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Venta());
            }

        }

        public ActionResult AltaFallo()
        {
            try
            {
                if (!(Session["Logueo"] is Empleado))
                    return RedirectToAction("FormLogueo", "Empleados");

                ViewBag.Mensaje = Session["Error"].ToString();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }

        }
    }
}

        





