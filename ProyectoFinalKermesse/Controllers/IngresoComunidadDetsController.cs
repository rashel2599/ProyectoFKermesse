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
    public class IngresoComunidadDetsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: IngresoComunidadDets
        public ActionResult Index(String valorBusq = "")
        {
            var ingresoComunidadDet = from ic in db.IngresoComunidadDet select ic;
            ingresoComunidadDet = db.IngresoComunidadDet.Include(i => i.ControlBono).Include(i => i.IngresoComunidad1);
            
            if (!String.IsNullOrEmpty(valorBusq))
            {
                ingresoComunidadDet = ingresoComunidadDet.Where(c => c.ControlBono.nombre.Contains(valorBusq));
            }

            ViewBag.Rol = Session["rol"];

            return View(ingresoComunidadDet.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteIngresoComunidadDet(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptIngresoComunidadDet.rdlc");
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

            var ingresoComunidadDet = from ic in db.IngresoComunidadDet select ic;
            ingresoComunidadDet = db.IngresoComunidadDet.Include(i => i.ControlBono).Include(i => i.IngresoComunidad1);

            if (!String.IsNullOrEmpty(valorBusq))
            {
                ingresoComunidadDet = ingresoComunidadDet.Where(c => c.ControlBono.nombre.Contains(valorBusq));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<IngresoComunidadDet> listaICDet = new List<IngresoComunidadDet>();
            listaICDet = ingresoComunidadDet.ToList();

            ReportDataSource rds = new ReportDataSource("DsIngresoComunidadDet", listaICDet);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteIngresoComunidadDetDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var ingresoComunidadDet = from ic in db.IngresoComunidadDet select ic;
            ingresoComunidadDet = ingresoComunidadDet.Where(ic => ic.idIngresoComunidadDet.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptIngresoComunidadDetDetalle.rdlc");
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

            List<IngresoComunidadDet> listaICDet = new List<IngresoComunidadDet>();
            listaICDet = modelo.IngresoComunidadDet.ToList();

            ReportDataSource rds = new ReportDataSource("DsIngresoComunidadDet", ingresoComunidadDet.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        // GET: IngresoComunidadDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidadDet ingresoComunidadDet = db.IngresoComunidadDet.Find(id);
            if (ingresoComunidadDet == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidadDet);
        }

        // GET: IngresoComunidadDets/Create
        public ActionResult Create()
        {
            ViewBag.bono = new SelectList(db.ControlBono, "idBono", "nombre");
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad");
            return View();
        }

        // POST: IngresoComunidadDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IngresoComunidadDet ingresoComunidadDet)
        {
            if (ModelState.IsValid)
            {
                var ic = new IngresoComunidadDet();

                ic.ingresoComunidad = ingresoComunidadDet.ingresoComunidad;
                ic.bono = ingresoComunidadDet.bono;
                ic.denominacion = ingresoComunidadDet.denominacion;
                ic.cantidad = ingresoComunidadDet.cantidad;
                ic.subTotalBono = ingresoComunidadDet.subTotalBono;


                db.IngresoComunidadDet.Add(ic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.bono = new SelectList(db.ControlBono, "idBono", "nombre", ingresoComunidadDet.bono);
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad", ingresoComunidadDet.ingresoComunidad);
            return View(ingresoComunidadDet);
        }

        // GET: IngresoComunidadDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidadDet ingresoComunidadDet = db.IngresoComunidadDet.Find(id);
            if (ingresoComunidadDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.bono = new SelectList(db.ControlBono, "idBono", "nombre", ingresoComunidadDet.bono);
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad", ingresoComunidadDet.ingresoComunidad);
            return View(ingresoComunidadDet);
        }

        // POST: IngresoComunidadDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IngresoComunidadDet ingresoComunidadDet)
        {
            if (ModelState.IsValid)
            {
                var ic = new IngresoComunidadDet();

                ic.idIngresoComunidadDet = ingresoComunidadDet.idIngresoComunidadDet;
                ic.ingresoComunidad = ingresoComunidadDet.ingresoComunidad;
                ic.bono = ingresoComunidadDet.bono;
                ic.denominacion = ingresoComunidadDet.denominacion;
                ic.cantidad = ingresoComunidadDet.cantidad;
                ic.subTotalBono = ingresoComunidadDet.subTotalBono;

                db.Entry(ingresoComunidadDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bono = new SelectList(db.ControlBono, "idBono", "nombre", ingresoComunidadDet.bono);
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad", ingresoComunidadDet.ingresoComunidad);
            return View(ingresoComunidadDet);
        }

        // GET: IngresoComunidadDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidadDet ingresoComunidadDet = db.IngresoComunidadDet.Find(id);
            if (ingresoComunidadDet == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidadDet);
        }

        // POST: IngresoComunidadDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                IngresoComunidadDet ingresoComunidadDet = db.IngresoComunidadDet.Find(id);
                db.IngresoComunidadDet.Remove(ingresoComunidadDet);
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
