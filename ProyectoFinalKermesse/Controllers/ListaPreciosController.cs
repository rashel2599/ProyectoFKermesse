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
    public class ListaPreciosController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ListaPrecios
        public ActionResult Index(string valorBusq = "")
        {
            var listaPrecio = from lp in db.ListaPrecio select lp;
            listaPrecio = db.ListaPrecio.Include(l => l.Kermesse1);
            listaPrecio = listaPrecio.Where(lp => lp.estado.Equals(1) || lp.estado.Equals(2));
            

            if (!string.IsNullOrEmpty(valorBusq))
            {
                listaPrecio = listaPrecio.Where(lp => lp.nombre.Contains(valorBusq));
            }

            
            return View(listaPrecio.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteListaPrecio(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptListaPrecio.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <OutputFormat>EMF</OutputFormat>
                      <PageWidth>8.5in</PageWidth>
                      <PageHeight>11in</PageHeight>
                      <MarginTop>0.25in</MarginTop>
                      <MarginLeft>0.25in</MarginLeft>
                      <MarginRight>0.25in</MarginRight>
                      <EmbedFonts>None</EmbedFonts>
                      <MarginBottom>0.25in</MarginBottom>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var listaPrecio = from lp in db.ListaPrecio select lp;
            listaPrecio = db.ListaPrecio.Include(l => l.Kermesse1);
            listaPrecio = listaPrecio.Where(lp => lp.estado.Equals(1) || lp.estado.Equals(2));


            if (!string.IsNullOrEmpty(valorBusq))
            {
                listaPrecio = listaPrecio.Where(lp => lp.nombre.Contains(valorBusq));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<ListaPrecio> ListaPrecio = new List<ListaPrecio>();
            ListaPrecio = listaPrecio.ToList();

            ReportDataSource rds = new ReportDataSource("DsListaPrecio", listaPrecio);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteListaPrecioDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptListaPrecioDetalle.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <OutputFormat>EMF</OutputFormat>
                      <PageWidth>8.5in</PageWidth>
                      <PageHeight>11in</PageHeight>
                      <MarginTop>0.25in</MarginTop>
                      <MarginLeft>0.25in</MarginLeft>
                      <MarginRight>0.25in</MarginRight>
                      <EmbedFonts>None</EmbedFonts>
                      <MarginBottom>0.25in</MarginBottom>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var listaPrecio = from lp in db.ListaPrecio select lp;
            listaPrecio = listaPrecio.Where(lp => lp.idListaPrecio.Equals(id));




            BDKermesseEntities modelo = new BDKermesseEntities();

            List<ListaPrecio> ListaPrecio = new List<ListaPrecio>();
            ListaPrecio = modelo.ListaPrecio.ToList();

            ReportDataSource rds = new ReportDataSource("DsListaPrecio", listaPrecio.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }


        // GET: ListaPrecios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);
            if (listaPrecio == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecio);
        }

        // GET: ListaPrecios/Create
        public ActionResult Create()
        {
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            return View();
        }

        // POST: ListaPrecios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ListaPrecio listaPrecio)
        {
            if (ModelState.IsValid)
            {
                var lp = new ListaPrecio();
                lp.nombre = listaPrecio.nombre;
                lp.descripcion = listaPrecio.descripcion;
                lp.kermesse = listaPrecio.kermesse;
                lp.estado = 1;

                db.ListaPrecio.Add(lp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", listaPrecio.kermesse);
            return View(listaPrecio);
        }

        // GET: ListaPrecios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);
            if (listaPrecio == null)
            {
                return HttpNotFound();
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", listaPrecio.kermesse);
            return View(listaPrecio);
        }

        // POST: ListaPrecios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ListaPrecio listaPrecio)
        {
            if (ModelState.IsValid)
            {
                var lp = new ListaPrecio();
                lp.idListaPrecio = listaPrecio.idListaPrecio;
                lp.nombre = listaPrecio.nombre;
                lp.descripcion = listaPrecio.descripcion;
                lp.kermesse = listaPrecio.kermesse;
                lp.estado = 2;

                db.Entry(lp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", listaPrecio.kermesse);
            return View(listaPrecio);
        }

        // GET: ListaPrecios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);
            if (listaPrecio == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecio);
        }

        // POST: ListaPrecios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);
            listaPrecio.estado = 3;

            db.Entry(listaPrecio).State = EntityState.Modified;
            db.SaveChanges();
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
