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
    public class TasaCambioDetsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: TasaCambioDets
        public ActionResult Index(string valorB= "")
        {
            var tasaCambioDet = db.TasaCambioDet.Include(t => t.TasaCambio1);

            var tasadt = from tdt in db.TasaCambioDet select tdt;
            tasadt = tasadt.Where(tdt => tdt.estado.Equals(2) || tdt.estado.Equals(1));

            /*if (!string.IsNullOrEmpty(valorB))
            {
                tasadt = tasadt.Where(tdt => tdt.tasaCambio.Contains(valorB));
            }*/


            return View(tasadt.ToList());
        }

        // GET: TasaCambioDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            if (tasaCambioDet == null)
            {
                return HttpNotFound();
            }
            return View(tasaCambioDet);
        }

        // GET: TasaCambioDets/Create
        public ActionResult Create()
        {
            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes");
            return View();
        }

        // POST: TasaCambioDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TasaCambioDet tasaCambioDet)
        {
            if (ModelState.IsValid)
            {
                var tdt = new TasaCambioDet();
                tdt.idTasaCambioDet = tasaCambioDet.idTasaCambioDet;
                tdt.tasaCambio = tasaCambioDet.tasaCambio;
                tdt.fecha = tasaCambioDet.fecha;
                tdt.tipoCambio = tasaCambioDet.tipoCambio;
                tdt.estado = 1;

                db.TasaCambioDet.Add(tdt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
            return View(tasaCambioDet);
        }

        // GET: TasaCambioDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            if (tasaCambioDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
            return View(tasaCambioDet);
        }

        // POST: TasaCambioDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TasaCambioDet tasaCambioDet)
        {
            if (ModelState.IsValid)
            {
                var tdt = new TasaCambioDet();
                tdt.idTasaCambioDet = tasaCambioDet.idTasaCambioDet;
                tdt.tasaCambio = tasaCambioDet.tasaCambio;
                tdt.fecha = tasaCambioDet.fecha;
                tdt.tipoCambio = tasaCambioDet.tipoCambio;
                tdt.estado = 2;

                db.Entry(tdt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
            return View(tasaCambioDet);
        }

        // GET: TasaCambioDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            if (tasaCambioDet == null)
            {
                return HttpNotFound();
            }
            return View(tasaCambioDet);
        }

        // POST: TasaCambioDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {


            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            tasaCambioDet.estado = 3;


            db.TasaCambioDet.Remove(tasaCambioDet);
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
