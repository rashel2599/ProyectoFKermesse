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
    public class RolUsuariosController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: RolUsuarios
        public ActionResult Index()
        {
            var rolUsuario = db.RolUsuario.Include(r => r.Rol1).Include(r => r.Usuario1);
            return View(rolUsuario.ToList());
        }

        // GET: RolUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolUsuario rolUsuario = db.RolUsuario.Find(id);
            if (rolUsuario == null)
            {
                return HttpNotFound();
            }
            return View(rolUsuario);
        }

        // GET: RolUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion");
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: RolUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRolUsuario,usuario,rol")] RolUsuario rolUsuario)
        {
            if (ModelState.IsValid)
            {
                db.RolUsuario.Add(rolUsuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolUsuario.rol);
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName", rolUsuario.usuario);
            return View(rolUsuario);
        }

        // GET: RolUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolUsuario rolUsuario = db.RolUsuario.Find(id);
            if (rolUsuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolUsuario.rol);
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName", rolUsuario.usuario);
            return View(rolUsuario);
        }

        // POST: RolUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRolUsuario,usuario,rol")] RolUsuario rolUsuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolUsuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.rol = new SelectList(db.Rol, "idRol", "rolDescripcion", rolUsuario.rol);
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName", rolUsuario.usuario);
            return View(rolUsuario);
        }

        // GET: RolUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolUsuario rolUsuario = db.RolUsuario.Find(id);
            if (rolUsuario == null)
            {
                return HttpNotFound();
            }
            return View(rolUsuario);
        }

        // POST: RolUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RolUsuario rolUsuario = db.RolUsuario.Find(id);
            db.RolUsuario.Remove(rolUsuario);
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
