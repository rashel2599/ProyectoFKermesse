using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinalKermesse.Models;

namespace ProyectoFinalKermesse.Controllers
{
    public class TasaCambiosController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: TasaCambios
        public ActionResult Index(string valorBusq = "")
        {
            var tasaCambio = from tc in db.TasaCambio select tc; 
                
            tasaCambio = db.TasaCambio.Include(t => t.Moneda).Include(t => t.Moneda1);
            tasaCambio = tasaCambio.Where(tc => tc.estado.Equals(2) || tc.estado.Equals(1));

            if (!string.IsNullOrEmpty(valorBusq))
            {
                tasaCambio = tasaCambio.Where(t => t.mes.Contains(valorBusq));
            }

            return View(tasaCambio.ToList());
        }

        // GET: TasaCambios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambio tasaCambio = db.TasaCambio.Find(id);
            if (tasaCambio == null)
            {
                return HttpNotFound();
            }
            return View(tasaCambio);
        }

        // GET: TasaCambios/Create
        public ActionResult Create()
        {
            ViewBag.monedaO = new SelectList(db.Moneda, "idMoneda", "nombre");
            ViewBag.monedaC = new SelectList(db.Moneda, "idMoneda", "nombre");
            return View();
        }

        // POST: TasaCambios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TasaCambio tasaCambio)
        {
            if (ModelState.IsValid)
            {
                var tc = new TasaCambio();
                tc.idTasaCambio = tasaCambio.idTasaCambio;
                tc.monedaO = tasaCambio.monedaO;
                tc.monedaC = tasaCambio.monedaC;
                tc.mes = tasaCambio.mes;
                tc.anio = tasaCambio.anio;
                tc.estado = 1;

                db.TasaCambio.Add(tc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.monedaO = new SelectList(db.Moneda, "idMoneda", "nombre", tasaCambio.monedaO);
            ViewBag.monedaC = new SelectList(db.Moneda, "idMoneda", "nombre", tasaCambio.monedaC);
            return View(tasaCambio);
        }

        // GET: TasaCambios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambio tasaCambio = db.TasaCambio.Find(id);
            if (tasaCambio == null)
            {
                return HttpNotFound();
            }
            ViewBag.monedaO = new SelectList(db.Moneda, "idMoneda", "nombre", tasaCambio.monedaO);
            ViewBag.monedaC = new SelectList(db.Moneda, "idMoneda", "nombre", tasaCambio.monedaC);
            return View(tasaCambio);
        }

        // POST: TasaCambios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TasaCambio tasaCambio)
        {
            if (ModelState.IsValid)
            {
                var tc = new TasaCambio();
                tc.idTasaCambio = tasaCambio.idTasaCambio;
                tc.monedaO = tasaCambio.monedaO;
                tc.monedaC = tasaCambio.monedaC;
                tc.mes = tasaCambio.mes;
                tc.anio = tasaCambio.anio;
                tc.estado = 2;


                db.Entry(tc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.monedaO = new SelectList(db.Moneda, "idMoneda", "nombre", tasaCambio.monedaO);
            ViewBag.monedaC = new SelectList(db.Moneda, "idMoneda", "nombre", tasaCambio.monedaC);
            return View(tasaCambio);
        }

        // GET: TasaCambios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambio tasaCambio = db.TasaCambio.Find(id);
            if (tasaCambio == null)
            {
                return HttpNotFound();
            }
            return View(tasaCambio);
        }

        // POST: TasaCambios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TasaCambio tasaCambio = db.TasaCambio.Find(id);
                tasaCambio.estado = 3;
                db.TasaCambio.Remove(tasaCambio);
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
