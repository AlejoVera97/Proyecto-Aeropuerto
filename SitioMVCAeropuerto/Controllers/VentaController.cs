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
        public ActionResult Index()
        {
            return View();
        }

        //---- ALTA VENTA 

        [HttpGet]
        public ActionResult FormAltaVenta()
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
        public ActionResult FormAltaVenta(Venta V)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logeo"];
                _E.ValidarEmpleado();
                V.ValidarVenta();


                FabricaLogica.GetLogicaVenta().AltaVenta(V, _E);
                return RedirectToAction("Formulario agregar", "Venta");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }


        //---- LISTAR VENTA 
        public ActionResult ListarVenta(int IDVenta)
        {
            try
            {
                Empleado _E = (Empleado)Session["Logeo"];
                 
                //obtengo lista para la vista (facturas)
                List<Venta> _lista = null;
                
                //obtengo lista de Ventas
                if (Session["Lista"] == null)
                {
                    _lista = FabricaLogica.GetLogicaVenta().ListarVentas(V, _E);
                    Session["Lista"] = _lista;
                }
                else
                    _lista = (List<Venta>)Session["Lista"];

                //no hay ventas
                if (_lista.Count == 0)
                    throw new Exception("No hay ventas  para mostar");

                //obtengo lista para el drop de articulos filtro 
                //*************
                List<Articulo> _ListaA = new ArticulosBD().ListarArticulo();
                _ListaA.Insert(0, new Articulo(0, "Seleccione", 0)); //parche para que no quede articulo seleccionado
                ViewBag.ListaA = new SelectList(_ListaA, "Codigo", "Nombre");


                //filtros o no
                if (!String.IsNullOrEmpty(FechaFiltro))
                {
                    _lista = (from unaF in _lista
                              where unaF.Fecha.Date == Convert.ToDateTime(FechaFiltro).Date
                              select unaF).ToList();
                }



            }
}
    }
}


