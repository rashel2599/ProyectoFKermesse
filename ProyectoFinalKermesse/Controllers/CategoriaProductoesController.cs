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
    public class CategoriaProductoesController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: CategoriaProductoes
        public ActionResult Index(string valorBusq = "")
        {
            var categoriaProducto = from ca in db.CategoriaProducto select ca;

            categoriaProducto = categoriaProducto.Where(ca => ca.estado.Equals(1) || ca.estado.Equals(2));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                categoriaProducto = categoriaProducto.Where(ca => ca.nombre.Contains(valorBusq));
            }

            return View(categoriaProducto.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteCatProd(string tipo, string valorBusq = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptCatProd.rdlc");

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

            var categoriaProducto = from ca in db.CategoriaProducto select ca;

            categoriaProducto = categoriaProducto.Where(ca => ca.estado.Equals(1) || ca.estado.Equals(2));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                categoriaProducto = categoriaProducto.Where(ca => ca.nombre.Contains(valorBusq));
            }


            BDKermesseEntities modelo = new BDKermesseEntities();

            List<CategoriaProducto> listaCatProd = new List<CategoriaProducto>();
            listaCatProd = categoriaProducto.ToList();

            ReportDataSource rds = new ReportDataSource("DsCatProd", listaCatProd);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteCatProdDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var categoriaProducto = from ca in db.CategoriaProducto select ca;
            categoriaProducto = categoriaProducto.Where(m => m.idCatProd.Equals(id));

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptCatProdDetalle.rdlc");

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

            List<CategoriaProducto> listaCatProd = new List<CategoriaProducto>();
            listaCatProd = modelo.CategoriaProducto.ToList();

            ReportDataSource rds = new ReportDataSource("DsCatProd", categoriaProducto.ToList());
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }



        // GET: CategoriaProductoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaProductoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriaProducto categoriaProducto)
        {
            if (ModelState.IsValid)
            {
                var ca = new CategoriaProducto();
                ca.nombre = categoriaProducto.nombre;
                ca.descripcion = categoriaProducto.descripcion;
                ca.estado = 1;

                db.CategoriaProducto.Add(ca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriaProducto categoriaProducto)
        {
            if (ModelState.IsValid)
            {
                var ca = new CategoriaProducto();
                ca.idCatProd = categoriaProducto.idCatProd;
                ca.nombre = categoriaProducto.nombre;
                ca.descripcion = categoriaProducto.descripcion;
                ca.estado = 2;

                db.Entry(ca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            categoriaProducto.estado = 3;

            db.Entry(categoriaProducto).State = EntityState.Modified;

            //db.CategoriaProducto.Remove(categoriaProducto);
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
