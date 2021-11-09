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
    public class IngresoComunidadDetsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: IngresoComunidadDets
        public ActionResult Index()
        {
            var ingresoComunidadDet = db.IngresoComunidadDet.Include(i => i.ControlBono).Include(i => i.IngresoComunidad1);
            return View(ingresoComunidadDet.ToList());
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
        public ActionResult Create([Bind(Include = "idIngresoComunidadDet,ingresoComunidad,bono,denominacion,cantidad,subTotalBono")] IngresoComunidadDet ingresoComunidadDet)
        {
            if (ModelState.IsValid)
            {
                db.IngresoComunidadDet.Add(ingresoComunidadDet);
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
        public ActionResult Edit([Bind(Include = "idIngresoComunidadDet,ingresoComunidad,bono,denominacion,cantidad,subTotalBono")] IngresoComunidadDet ingresoComunidadDet)
        {
            if (ModelState.IsValid)
            {
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
            IngresoComunidadDet ingresoComunidadDet = db.IngresoComunidadDet.Find(id);
            db.IngresoComunidadDet.Remove(ingresoComunidadDet);
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
