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
    public class RolOpcionsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: RolOpcions
        public ActionResult Index(String valorBusq = "")
        {
            var rolOpcion = from r in db.RolOpcion select r;

            rolOpcion = db.RolOpcion.Include(r => r.Opcion1).Include(r => r.Rol1);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                rolOpcion = rolOpcion.Where(r => r.Opcion1.opcionDescripcion.Contains(valorBusq));
            }


            return View(rolOpcion.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteRolOpcion(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptRolOpcion.rdlc");
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

            var rolOpcion = from r in db.RolOpcion select r;

            rolOpcion = db.RolOpcion.Include(r => r.Opcion1).Include(r => r.Rol1);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                rolOpcion = rolOpcion.Where(r => r.Opcion1.opcionDescripcion.Contains(valorBusq));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<RolOpcion> listaRolOp = new List<RolOpcion>();
            listaRolOp = rolOpcion.ToList();

            ReportDataSource rds = new ReportDataSource("DsRolOpcion", listaRolOp);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteRolOpcionDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var rolOpcion = from r in db.RolOpcion select r;
            rolOpcion = rolOpcion.Where(r => r.idRolOpcion.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptRolOpcionDetalle.rdlc");
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

            List<RolOpcion> listaRolOp = new List<RolOpcion>();
            listaRolOp = modelo.RolOpcion.ToList();

            ReportDataSource rds = new ReportDataSource("DsRolOpcion", rolOpcion.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }


        // GET: RolOpcions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcion);
        }

        // GET: RolOpcions/Create
        public ActionResult Create()
        {
            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion");
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion");
            return View();
        }

        // POST: RolOpcions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolOpcion rolOpcion)
        {
            if (ModelState.IsValid)
            {
                var r = new RolOpcion();

                r.opcion = rolOpcion.opcion;
                r.rol = rolOpcion.rol;

                db.RolOpcion.Add(r);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // GET: RolOpcions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // POST: RolOpcions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRolOpcion,rol,opcion")] RolOpcion rolOpcion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolOpcion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // GET: RolOpcions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcion);
        }

        // POST: RolOpcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                RolOpcion rolOpcion = db.RolOpcion.Find(id);
                db.RolOpcion.Remove(rolOpcion);
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
