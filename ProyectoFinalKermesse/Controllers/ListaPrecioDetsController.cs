using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using ProyectoFinalKermesse.Models;

namespace ProyectoFinalKermesse.Controllers
{
    public class ListaPrecioDetsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ListaPrecioDets
        public ActionResult Index(string valorBusq = "")
        {
            var listaPrecioDet = from l in db.ListaPrecioDet select l;
            listaPrecioDet = db.ListaPrecioDet.Include(l => l.ListaPrecio1).Include(l => l.Producto1);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                listaPrecioDet = listaPrecioDet.Where(l => l.ListaPrecio1.nombre.Contains(valorBusq));
            }


            return View(listaPrecioDet.ToList());
        }
        //Get: VerReportes

        public ActionResult VerReporteListaPrecioDet(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptListaPrecioDet.rdlc");
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

            var vwlistaPrecioDet = from l in db.VwListaPrecioDet select l;



            if (!string.IsNullOrEmpty(valorBusq))
            {
                vwlistaPrecioDet = vwlistaPrecioDet.Where(l => l.ListaPrecio.Contains(valorBusq));
            }


            //BDKermesseEntities modelo = new BDKermesseEntities();

            List<VwListaPrecioDet> listaPrecioDet = new List<VwListaPrecioDet>();
            listaPrecioDet = vwlistaPrecioDet.ToList();

            ReportDataSource rds = new ReportDataSource("DsListaPrecioDet", listaPrecioDet);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }
        //Get: VerReportesDetalle

        public ActionResult VerReporteListaPrecioDetDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var vwlistaPrecioDet = from l in db.VwListaPrecioDet select l;
            vwlistaPrecioDet = vwlistaPrecioDet.Where(l => l.id.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptListaPrecioDetDetalle.rdlc");
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

            List<VwListaPrecioDet> listaPrecioDet = new List<VwListaPrecioDet>();
            listaPrecioDet = modelo.VwListaPrecioDet.ToList();

            ReportDataSource rds = new ReportDataSource("DsListaPrecioDet", vwlistaPrecioDet.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }
        // GET: ListaPrecioDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Create
        public ActionResult Create()
        {
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");
            return View();
        }

        // POST: ListaPrecioDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ListaPrecioDet listaPrecioDet)
        {
            if (ModelState.IsValid)
            {
                var l = new ListaPrecioDet();
                l.listaPrecio = listaPrecioDet.listaPrecio;
                l.producto = listaPrecioDet.producto;
                l.precioVenta = listaPrecioDet.precioVenta;


                db.ListaPrecioDet.Add(l);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // POST: ListaPrecioDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idListaPrecioDet,listaPrecio,producto,precioVenta")] ListaPrecioDet listaPrecioDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listaPrecioDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecioDet);
        }

        // POST: ListaPrecioDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
                db.ListaPrecioDet.Remove(listaPrecioDet);
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
