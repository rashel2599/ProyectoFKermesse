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
    public class ArqueoCajaDetsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ArqueoCajaDets
        public ActionResult Index(string valorBusq = "")
        {
            var arqueoCajaDet = from ac in db.ArqueoCajaDet select ac; 
            
            arqueoCajaDet = db.ArqueoCajaDet.Include(a => a.ArqueoCaja1).Include(a => a.Denominacion1).Include(a => a.Moneda1);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                arqueoCajaDet = arqueoCajaDet.Where(a => a.Moneda1.nombre.Contains(valorBusq));
            }

            ViewBag.Rol = Session["rol"];
            return View(arqueoCajaDet.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteArqueoCajaDet(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptArqueoCajaDet.rdlc");
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

            var vwarqueoCajaDet = from ac in db.VwArqueoCajaDet select ac;



            if (!string.IsNullOrEmpty(valorBusq))
            {
                vwarqueoCajaDet = vwarqueoCajaDet.Where(ac => ac.Moneda.Contains(valorBusq));
            }


            //BDKermesseEntities modelo = new BDKermesseEntities();

            List<VwArqueoCajaDet> listaArqueoDet = new List<VwArqueoCajaDet>();
            listaArqueoDet = vwarqueoCajaDet.ToList();

            ReportDataSource rds = new ReportDataSource("DsArqueoCajaDet", listaArqueoDet);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteArqueoCajaDetDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var vwarqueoCajaDet = from ac in db.VwArqueoCajaDet select ac;
            vwarqueoCajaDet = vwarqueoCajaDet.Where(ac => ac.id.Equals(id));


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptArqueoCajaDetDetalle.rdlc");
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

            List<VwArqueoCajaDet> listaArqueoDet = new List<VwArqueoCajaDet>();
            listaArqueoDet = modelo.VwArqueoCajaDet.ToList();

            ReportDataSource rds = new ReportDataSource("DsArqueoCajaDet", vwarqueoCajaDet.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }


        // GET: ArqueoCajaDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Create
        public ActionResult Create()
        {
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja");
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras");
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre");
            return View();
        }

        // POST: ArqueoCajaDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idArqueoCajaDet,arqueoCaja,moneda,denominacion,cantidad,subtotal")] ArqueoCajaDet arqueoCajaDet)
        {
            if (ModelState.IsValid)
            {
                db.ArqueoCajaDet.Add(arqueoCajaDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // POST: ArqueoCajaDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idArqueoCajaDet,arqueoCaja,moneda,denominacion,cantidad,subtotal")] ArqueoCajaDet arqueoCajaDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arqueoCajaDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCajaDet);
        }

        // POST: ArqueoCajaDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
                db.ArqueoCajaDet.Remove(arqueoCajaDet);
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
