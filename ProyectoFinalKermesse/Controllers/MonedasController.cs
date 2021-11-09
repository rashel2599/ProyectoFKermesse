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
    public class MonedasController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Monedas
        public ActionResult Index(string valorBusq = "")
        {
            var moneda = from m in db.Moneda select m;

            moneda = moneda.Where(m => m.estado.Equals(2) || m.estado.Equals(1));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                moneda = moneda.Where(m => m.nombre.Contains(valorBusq));
            }


            return View(moneda.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteMoneda(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptMoneda.rdlc");
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

            var moneda = from m in db.Moneda select m;

            moneda = moneda.Where(m => m.estado.Equals(2) || m.estado.Equals(1));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                moneda = moneda.Where(m => m.nombre.Contains(valorBusq));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Moneda> listaMon = new List<Moneda>();
            listaMon = moneda.ToList();

            ReportDataSource rds = new ReportDataSource("DsMoneda", listaMon);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteMonedaDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var moneda = from m in db.Moneda select m;
            moneda = moneda.Where(m => m.idMoneda.Equals(id)); 


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptMonedaDetalle.rdlc");
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

            List<Moneda> listaMon = new List<Moneda>();
            listaMon = modelo.Moneda.ToList();

            ReportDataSource rds = new ReportDataSource("DsMoneda", moneda.ToList() );
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }


        // GET: Monedas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return HttpNotFound();
            }
            return View(moneda);
        }

        // GET: Monedas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Monedas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Moneda moneda)
        {
            if (ModelState.IsValid)
            {
                var m = new Moneda();
                m.nombre = moneda.nombre;
                m.simbolo = moneda.simbolo;
                m.estado = 1;

                db.Moneda.Add(m);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(moneda);
        }

        // GET: Monedas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return HttpNotFound();
            }
            return View(moneda);
        }

        // POST: Monedas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Moneda moneda)
        {
            if (ModelState.IsValid)
            {
                var m = new Moneda();
                m.idMoneda = moneda.idMoneda;
                m.nombre = moneda.nombre;
                m.simbolo = moneda.simbolo;
                m.estado = 2;

                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(moneda);
        }

        // GET: Monedas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return HttpNotFound();
            }
            return View(moneda);
        }

        // POST: Monedas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Moneda moneda = db.Moneda.Find(id);
            moneda.estado = 3;

            db.Entry(moneda).State = EntityState.Modified;
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
