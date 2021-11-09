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
    public class OpcionsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Opcions
        public ActionResult Index(string valorB= "")
        {
            var opciones = from op in db.Opcion select op;

            opciones = opciones.Where(op => op.estado.Equals(2) || op.estado.Equals(1));

            if (!string.IsNullOrEmpty(valorB))
            {
                opciones = opciones.Where(op => op.opcionDescripcion.Contains(valorB));
            }

            return View(opciones.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteOpcion(string tipo, string valorB = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptOpcion.rdlc");
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

            var opciones = from op in db.Opcion select op;

            opciones = opciones.Where(op => op.estado.Equals(2) || op.estado.Equals(1));

            if (!string.IsNullOrEmpty(valorB))
            {
                opciones = opciones.Where(op => op.opcionDescripcion.Contains(valorB));
            }

            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Opcion> listaOp = new List<Opcion>();
            listaOp = opciones.ToList();

            ReportDataSource rds = new ReportDataSource("DsOpcion", listaOp);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteOpcionDetalle(int id )
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var opciones = from op in db.Opcion select op;
            opciones = opciones.Where(op => op.idOpcion.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptOpcionDetalle.rdlc");
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



            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Opcion> listaOp = new List<Opcion>();
            listaOp = modelo.Opcion.ToList();

            ReportDataSource rds = new ReportDataSource("DsOpcion", opciones.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        // GET: Opcions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // GET: Opcions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Opcions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Opcion opcion)
        {
            if (ModelState.IsValid)
            {
                var op = new Opcion();
                op.opcionDescripcion = opcion.opcionDescripcion;
                op.estado = 1;

                db.Opcion.Add(op);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opcion);
        }

        // GET: Opcions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // POST: Opcions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Opcion opcion)
        {
            if (ModelState.IsValid)
            {
                var op = new Opcion();
                op.idOpcion = opcion.idOpcion;
                op.opcionDescripcion = opcion.opcionDescripcion;
                op.estado = 2;

                db.Entry(op).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcion);
        }

        // GET: Opcions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // POST: Opcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opcion opcion = db.Opcion.Find(id);
            opcion.estado = 3;

            db.Opcion.Remove(opcion);
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
