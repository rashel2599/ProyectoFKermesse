using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using ProyectoFinalKermesse.Models;

namespace ProyectoFinalKermesse.Controllers
{
    public class IngresoComunidadsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: IngresoComunidads
        public ActionResult Index(String valorBusq = "")
        {
            var ingresoComunidad = from ic in db.IngresoComunidad select ic;
            ingresoComunidad = db.IngresoComunidad.Include(i => i.Comunidad1).Include(i => i.Kermesse1).Include(i => i.Producto1).Include(i => i.Usuario).Include(i => i.Usuario1).Include(i => i.Usuario2);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                ingresoComunidad = ingresoComunidad.Where(c => c.Comunidad1.nombre.Contains(valorBusq));
            }

            ViewBag.Rol = Session["rol"];
            return View(ingresoComunidad.ToList());
        }



        //Get: VerReportes

        public ActionResult VerReporteIngresoComunidad(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptIngresoComunidad.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <OutputFormat>EMF</OutputFormat>
                      <PageWidth>21.59cm</PageWidth>
                      <PageHeight>27.94cm</PageHeight>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <EmbedFonts>None</EmbedFonts>
                      <MarginBottom>0cm</MarginBottom>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var vwingresoComunidad = from ic in db.VwIngresoComunidad select ic;
           
            if (!string.IsNullOrEmpty(valorBusq))
            {
                vwingresoComunidad = vwingresoComunidad.Where(c => c.Comunidad.Contains(valorBusq));
            }



            BDKermesseEntities modelo = new BDKermesseEntities();

            List<VwIngresoComunidad> listaIngCom = new List<VwIngresoComunidad>();
            listaIngCom = vwingresoComunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DsIngresoComunidad", listaIngCom);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteIngresoComunidadDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var vwingresoComunidad = from ic in db.VwIngresoComunidad select ic;
            vwingresoComunidad = vwingresoComunidad.Where(ic => ic.id.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptIngresoComunidadDetalle.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <OutputFormat>EMF</OutputFormat>
                      <PageWidth>21.59cm</PageWidth>
                      <PageHeight>27.94cm</PageHeight>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <EmbedFonts>None</EmbedFonts>
                      <MarginBottom>0cm</MarginBottom>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<VwIngresoComunidad> listaIngCom = new List<VwIngresoComunidad>();
            listaIngCom = modelo.VwIngresoComunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DsIngresoComunidad", vwingresoComunidad.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReporteResumen

        public ActionResult VerReporteIngresos(string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptResumenIngresos.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <OutputFormat>EMF</OutputFormat>
                      <PageWidth>21.59cm</PageWidth>
                      <PageHeight>27.94cm</PageHeight>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <EmbedFonts>None</EmbedFonts>
                      <MarginBottom>0cm</MarginBottom>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var vwingresoComunidad = from ic in db.VwIngresoComunidad select ic;

            if (!string.IsNullOrEmpty(valorBusq))
            {
                vwingresoComunidad = vwingresoComunidad.Where(c => c.Comunidad.Contains(valorBusq));
            }



            BDKermesseEntities modelo = new BDKermesseEntities();

            List<VwIngresoComunidad> listaIngCom = new List<VwIngresoComunidad>();
            listaIngCom = vwingresoComunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DsIngresoComunidad", listaIngCom);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);




        }


        // GET: IngresoComunidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Create
        public ActionResult Create()
        {
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre");
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: IngresoComunidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IngresoComunidad ingresoComunidad)
        {
            if (ModelState.IsValid)
            {
                var ic = new IngresoComunidad();

                ic.kermesse = ingresoComunidad.kermesse;
                ic.comunidad = ingresoComunidad.comunidad;
                ic.producto = ingresoComunidad.producto;
                ic.cantProducto = ingresoComunidad.cantProducto;
                ic.totalBonos = ingresoComunidad.totalBonos;
                ic.usuarioCreacion = Convert.ToInt32(Session["idUsuario"]);
                ic.fechaCreacion = DateTime.Today;

                db.IngresoComunidad.Add(ic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }


            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // POST: IngresoComunidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idIngresoComunidad,kermesse,comunidad,producto,cantProducto,totalBonos,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] IngresoComunidad ingresoComunidad)
        {
            if (ModelState.IsValid)
            {
                ingresoComunidad.fechaModificacion = DateTime.Today;
                ingresoComunidad.usuarioModificacion = Convert.ToInt32(Session["idUsuario"]);

                db.Entry(ingresoComunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidad);
        }

        // POST: IngresoComunidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
                db.IngresoComunidad.Remove(ingresoComunidad);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
