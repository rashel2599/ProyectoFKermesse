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
    public class ArqueoCajasController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ArqueoCajas
        public ActionResult Index(string valorBusq = "")
        {
            var arqueoCaja = from ac in db.ArqueoCaja select ac; 
                
            arqueoCaja = db.ArqueoCaja.Include(a => a.Kermesse1).Include(a => a.Usuario).Include(a => a.Usuario1).Include(a => a.Usuario2);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                arqueoCaja = arqueoCaja.Where(a => a.Kermesse1.nombre.Contains(valorBusq));
            }

            return View(arqueoCaja.ToList());
        }

        public ActionResult VerReporteArqueoCaja(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptArqueoCaja.rdlc");
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

            var vwarqueoCaja = from ac in db.VwArqueoCaja select ac;



            if (!string.IsNullOrEmpty(valorBusq))
            {
                vwarqueoCaja = vwarqueoCaja.Where(ac => ac.Kermesse.Contains(valorBusq));
            }


            //BDKermesseEntities modelo = new BDKermesseEntities();

            List<VwArqueoCaja> listaArqueo = new List<VwArqueoCaja>();
            listaArqueo = vwarqueoCaja.ToList();

            ReportDataSource rds = new ReportDataSource("DsArqueoCaja", listaArqueo);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteArqueoCajaDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var vwarqueoCaja = from ac in db.VwArqueoCaja select ac;
            vwarqueoCaja = vwarqueoCaja.Where(ac => ac.id.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptArqueoCajaDetalle.rdlc");
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

            List<VwArqueoCaja> listaArqueo = new List<VwArqueoCaja>();
            listaArqueo = modelo.VwArqueoCaja.ToList();

            ReportDataSource rds = new ReportDataSource("DsArqueoCaja", vwarqueoCaja.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }


        // GET: ArqueoCajas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Create
        public ActionResult Create()
        {
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: ArqueoCajas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idArqueoCaja,kermesse,fechaArqueo,granTotal,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] ArqueoCaja arqueoCaja)
        {
            if (ModelState.IsValid)
            {
                arqueoCaja.fechaCreacion = DateTime.Today;
                arqueoCaja.fechaArqueo = DateTime.Today;
                arqueoCaja.usuarioCreacion = Convert.ToInt32(Session["idUsuario"]);

                db.ArqueoCaja.Add(arqueoCaja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }

            ViewBag.FArqueo = arqueoCaja.fechaArqueo?.ToString("yyyy-MM-dd");

            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // POST: ArqueoCajas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idArqueoCaja,kermesse,fechaArqueo,granTotal,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] ArqueoCaja arqueoCaja)
        {
            if (ModelState.IsValid)
            {
                arqueoCaja.fechaModificacion = DateTime.Today;
                arqueoCaja.usuarioModificacion = Convert.ToInt32(Session["idUsuario"]);

                db.Entry(arqueoCaja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCaja);
        }

        // POST: ArqueoCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
                db.ArqueoCaja.Remove(arqueoCaja);
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
