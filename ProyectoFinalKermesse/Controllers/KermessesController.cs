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
    public class KermessesController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Kermesses
        public ActionResult Index(String valorBusq ="")
        {
            var kermesse = from k in db.Kermesse select k;
            kermesse = db.Kermesse.Include(k => k.Parroquia1).Include(k => k.Usuario).Include(k => k.Usuario1).Include(k => k.Usuario2);
            kermesse = kermesse.Where(k => k.estado.Equals(1) || k.estado.Equals(2));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                kermesse = kermesse.Where(k => k.nombre.Contains(valorBusq));
            }

            ViewBag.Rol = Session["rol"];
            return View(kermesse.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteKermesse(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptKermesse.rdlc");
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

            var vwkermesse = from k in db.VwKermesse select k;
         
           

            if (!string.IsNullOrEmpty(valorBusq))
            {
                vwkermesse = vwkermesse.Where(k => k.Kermesse.Contains(valorBusq));
            }


            //BDKermesseEntities modelo = new BDKermesseEntities();

            List<VwKermesse> listaKer = new List<VwKermesse>();
            listaKer = vwkermesse.ToList();

            ReportDataSource rds = new ReportDataSource("DsKermesse", listaKer);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteKermesseDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var vwkermesse = from k in db.VwKermesse select k;
            vwkermesse = vwkermesse.Where(k => k.idKermesse.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptKermesseDetalle.rdlc");
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

            List<VwKermesse> listaKer = new List<VwKermesse>();
            listaKer = modelo.VwKermesse.ToList();

            ReportDataSource rds = new ReportDataSource("DsKermesse", vwkermesse.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        // GET: Kermesses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            return View(kermesse);
        }

        // GET: Kermesses/Create
        public ActionResult Create()
        {
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: Kermesses/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
                var k = new Kermesse();

                k.nombre = kermesse.nombre;
                k.descripcion = kermesse.descripcion;
                k.parroquia = kermesse.parroquia;
                k.fInicio = kermesse.fInicio;
                k.fFinal = kermesse.fFinal;
                k.usuarioCreacion = Convert.ToInt32(Session["idUsuario"]);
                k.fechaCreacion = DateTime.Today;
                k.estado = 1;

                db.Kermesse.Add(k);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre", kermesse.parroquia);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioEliminacion);
            return View(kermesse);
        }

        // GET: Kermesses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }

            
            //DateTime da = Convert.ToDateTime(st);
            ViewBag.Finicio = kermesse.fInicio.ToString("yyyy-MM-dd");
            ViewBag.Ffinal = kermesse.fFinal.ToString("yyyy-MM-dd");
            //kermesse.fInicio = new DateTime(2021, 11, 21);

            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre", kermesse.parroquia);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioEliminacion);
            return View(kermesse);
        }

        // POST: Kermesses/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idKermesse,parroquia,nombre,fInicio,fFinal,descripcion,estado,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
                
                kermesse.estado = 2;
                kermesse.fechaModificacion = DateTime.Today;
                kermesse.usuarioModificacion = Convert.ToInt32(Session["idUsuario"]);

                db.Entry(kermesse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre", kermesse.parroquia);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioEliminacion);
            return View(kermesse);
        }

        // GET: Kermesses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            return View(kermesse);
        }

        // POST: Kermesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Kermesse kermesse = db.Kermesse.Find(id);

                kermesse.estado = 3;

                db.Entry(kermesse).State = EntityState.Modified;
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
