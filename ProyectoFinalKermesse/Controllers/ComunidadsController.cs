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
    public class ComunidadsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Comunidads
        public ActionResult Index(String valorBusq = "")
        {
            var comunidad = from c in db.Comunidad select c;
            comunidad = comunidad.Where(c => c.estado.Equals(1) || c.estado.Equals(2));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                comunidad = comunidad.Where(c => c.nombre.Contains(valorBusq));
            }

            return View(comunidad.ToList());
        }


        //Get: VerReportes

        public ActionResult VerReporteComunidad(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptComunidad.rdlc");
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

            var comunidad = from c in db.Comunidad select c;
            comunidad = comunidad.Where(c => c.estado.Equals(1) || c.estado.Equals(2));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                comunidad = comunidad.Where(c => c.nombre.Contains(valorBusq));
            }



            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Comunidad> listaCom = new List<Comunidad>();
            listaCom = comunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DsComunidad", listaCom);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }
        //Get: VerReportesDetalle

        public ActionResult VerReporteComunidadDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var comunidad = from c in db.Comunidad select c;
            comunidad = comunidad.Where(m => m.idComunidad.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptComunidadDetalle.rdlc");
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

            List<Comunidad> listaCom = new List<Comunidad>();
            listaCom = modelo.Comunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DsComunidad", comunidad.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        // GET: Comunidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // GET: Comunidads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comunidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comunidad comunidad)
        {
            if (ModelState.IsValid)
            {
                var cm = new Comunidad();
                cm.nombre = comunidad.nombre;
                cm.responsble = comunidad.responsble;
                cm.descContribucion = comunidad.descContribucion;
                cm.estado = 1;

                db.Comunidad.Add(cm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comunidad);
        }

        // GET: Comunidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // POST: Comunidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idComunidad,nombre,responsble,descContribucion,estado")] Comunidad comunidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comunidad);
        }

        // GET: Comunidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // POST: Comunidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comunidad comunidad = db.Comunidad.Find(id);
            db.Comunidad.Remove(comunidad);
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
