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
    public class ComunidadsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Comunidads
        public ActionResult Index()
        {
            return View(db.Comunidad.ToList());
        }

        // GET: Comunidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // GET: Comunidads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comunidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idComunidad,nombre,responsble,descContribucion,estado")] Comunidad comunidad)
        {
            if (ModelState.IsValid)
            {
                db.Comunidad.Add(comunidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comunidad);
        }

        // GET: Comunidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // POST: Comunidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idComunidad,nombre,responsble,descContribucion,estado")] Comunidad comunidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comunidad);
        }

        // GET: Comunidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // POST: Comunidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comunidad comunidad = db.Comunidad.Find(id);
            db.Comunidad.Remove(comunidad);
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
