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
    public class RolOpcionsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: RolOpcions
        public ActionResult Index()
        {
            var rolOpcion = db.RolOpcion.Include(r => r.Opcion1).Include(r => r.Rol1);
            return View(rolOpcion.ToList());
        }

        // GET: RolOpcions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcion);
        }

        // GET: RolOpcions/Create
        public ActionResult Create()
        {
            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion");
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion");
            return View();
        }

        // POST: RolOpcions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRolOpcion,rol,opcion")] RolOpcion rolOpcion)
        {
            if (ModelState.IsValid)
            {
                db.RolOpcion.Add(rolOpcion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // GET: RolOpcions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // POST: RolOpcions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRolOpcion,rol,opcion")] RolOpcion rolOpcion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolOpcion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.opcion = new SelectList(db.Opcion, "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // GET: RolOpcions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcion);
        }

        // POST: RolOpcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            db.RolOpcion.Remove(rolOpcion);
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
