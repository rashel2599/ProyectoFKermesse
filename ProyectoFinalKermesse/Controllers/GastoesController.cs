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
    public class GastoesController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Gastoes
        public ActionResult Index(String valorBusq = "")
        {

            var gasto = from g in db.Gasto select g;
            gasto = db.Gasto.Include(g => g.CategoriaGasto).Include(g => g.Kermesse1).Include(g => g.Usuario).Include(g => g.Usuario1).Include(g => g.Usuario2);


            if (!string.IsNullOrEmpty(valorBusq))
            {
                gasto = gasto.Where(lp => lp.concepto.Contains(valorBusq));
            }

            return View(gasto.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteGasto(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptGasto.rdlc");
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

            var gasto = from g in db.Gasto select g;
            gasto = db.Gasto.Include(g => g.CategoriaGasto).Include(g => g.Kermesse1).Include(g => g.Usuario).Include(g => g.Usuario1).Include(g => g.Usuario2);


            if (!string.IsNullOrEmpty(valorBusq))
            {
                gasto = gasto.Where(lp => lp.concepto.Contains(valorBusq));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Gasto> listaGas = new List<Gasto>();
            listaGas = gasto.ToList();

            ReportDataSource rds = new ReportDataSource("DsGasto", listaGas);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteGastoDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var gasto = from g in db.Gasto select g;
            gasto = gasto.Where(g => g.idGasto.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptGastoDetalle.rdlc");
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

            List<Gasto> listaGas = new List<Gasto>();
            listaGas = modelo.Gasto.ToList();

            ReportDataSource rds = new ReportDataSource("DsGasto", gasto.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }



        // GET: Gastoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gasto.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            return View(gasto);
        }

        // GET: Gastoes/Create
        public ActionResult Create()
        {
            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria");
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: Gastoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                var g = new Gasto();

                g.kermesse = gasto.kermesse;
                g.catGasto = gasto.catGasto;
                g.fechGasto = gasto.fechGasto;
                g.concepto = gasto.concepto;
                g.monto = gasto.monto;
                g.usuarioCreacion = gasto.usuarioCreacion;
                g.fechaCreacion = DateTime.Today;

                db.Gasto.Add(g);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria", gasto.catGasto);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", gasto.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioCreacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioEliminacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioModificacion);
            return View(gasto);
        }

        // GET: Gastoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gasto.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria", gasto.catGasto);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", gasto.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioCreacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioEliminacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioModificacion);
            return View(gasto);
        }

        // POST: Gastoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idGasto,kermesse,catGasto,fechGasto,concepto,monto,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                gasto.fechaModificacion = DateTime.Today;

                db.Entry(gasto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria", gasto.catGasto);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", gasto.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioCreacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioEliminacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioModificacion);
            return View(gasto);
        }

        // GET: Gastoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gasto.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            return View(gasto);
        }

        // POST: Gastoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gasto gasto = db.Gasto.Find(id);
            db.Gasto.Remove(gasto);
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
