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
    public class ControlBonoesController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ControlBonoes
        public ActionResult Index(string valorBusq="")
        {
            var controlBono = from cb in db.ControlBono select cb;
            
            if (!string.IsNullOrEmpty(valorBusq))
            {
                controlBono = controlBono.Where(c => c.nombre.Contains(valorBusq));
            }



            return View(controlBono.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteControlBono(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptControlBono.rdlc");
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

            var controlBono = from cb in db.ControlBono select cb;
            
            if (!string.IsNullOrEmpty(valorBusq))
            {
                controlBono = controlBono.Where(c => c.nombre.Contains(valorBusq));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<ControlBono> listaControlB = new List<ControlBono>();
            listaControlB = controlBono.ToList();

            ReportDataSource rds = new ReportDataSource("DsControlBono", listaControlB);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteControlBonoDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var controlBono = from cb in db.ControlBono select cb;
            controlBono = controlBono.Where(cb => cb.idBono.Equals(id));

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptControlBonoDetalle.rdlc");
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

            List<ControlBono> listaControlB = new List<ControlBono>();
            listaControlB = modelo.ControlBono.ToList();

            ReportDataSource rds = new ReportDataSource("DsControlBono", controlBono.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }


        // GET: ControlBonoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // GET: ControlBonoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControlBonoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idBono,nombre,valor,estado")] ControlBono controlBono)
        {
            if (ModelState.IsValid)
            {
                db.ControlBono.Add(controlBono);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(controlBono);
        }

        // GET: ControlBonoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // POST: ControlBonoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idBono,nombre,valor,estado")] ControlBono controlBono)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controlBono).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(controlBono);
        }

        // GET: ControlBonoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // POST: ControlBonoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ControlBono controlBono = db.ControlBono.Find(id);
            db.ControlBono.Remove(controlBono);
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
