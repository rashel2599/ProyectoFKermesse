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
    public class ControlBonoesController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ControlBonoes
        public ActionResult Index()
        {
            return View(db.ControlBono.ToList());
        }

        // GET: ControlBonoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // GET: ControlBonoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControlBonoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idBono,nombre,valor,estado")] ControlBono controlBono)
        {
            if (ModelState.IsValid)
            {
                db.ControlBono.Add(controlBono);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(controlBono);
        }

        // GET: ControlBonoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // POST: ControlBonoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idBono,nombre,valor,estado")] ControlBono controlBono)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controlBono).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(controlBono);
        }

        // GET: ControlBonoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // POST: ControlBonoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ControlBono controlBono = db.ControlBono.Find(id);
            db.ControlBono.Remove(controlBono);
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
