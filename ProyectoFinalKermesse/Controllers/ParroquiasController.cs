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
    public class ParroquiasController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Parroquias
        public ActionResult Index(string valorB = "")
        {
            var parroquia = from pa in db.Parroquia select pa;

            if (!string.IsNullOrEmpty(valorB))
            {
                parroquia = parroquia.Where(pa => pa.nombre.Contains(valorB));
            }

            return View(parroquia.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteParroquia(string tipo, string valorB = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptParroquia.rdlc");
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

            var parroquia = from pa in db.Parroquia select pa;

            if (!string.IsNullOrEmpty(valorB))
            {
                parroquia = parroquia.Where(pa => pa.nombre.Contains(valorB));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Parroquia> listaParroquia = new List<Parroquia>();
            listaParroquia = parroquia.ToList();

            ReportDataSource rds = new ReportDataSource("DsParroquia", listaParroquia);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteParroquiaDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var parroquia = from pa in db.Parroquia select pa;
            parroquia = parroquia.Where(pa => pa.idParroquia.Equals(id));

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptParroquiaDetalle.rdlc");
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

            List<Parroquia> listaParroquia = new List<Parroquia>();
            listaParroquia = modelo.Parroquia.ToList();

            ReportDataSource rds = new ReportDataSource("DsParroquia", parroquia.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }


        // GET: Parroquias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parroquia parroquia = db.Parroquia.Find(id);
            if (parroquia == null)
            {
                return HttpNotFound();
            }
            return View(parroquia);
        }

        // GET: Parroquias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parroquias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idParroquia,nombre,direccion,telefono,parroco,logo,sitioWeb")] Parroquia parroquia)
        {
            if (ModelState.IsValid)
            {
                db.Parroquia.Add(parroquia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parroquia);
        }

        // GET: Parroquias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parroquia parroquia = db.Parroquia.Find(id);
            if (parroquia == null)
            {
                return HttpNotFound();
            }
            return View(parroquia);
        }

        // POST: Parroquias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idParroquia,nombre,direccion,telefono,parroco,logo,sitioWeb")] Parroquia parroquia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parroquia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parroquia);
        }

        // GET: Parroquias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parroquia parroquia = db.Parroquia.Find(id);
            if (parroquia == null)
            {
                return HttpNotFound();
            }
            return View(parroquia);
        }

        // POST: Parroquias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parroquia parroquia = db.Parroquia.Find(id);
            db.Parroquia.Remove(parroquia);
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
