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
    public class IngresoComunidadsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: IngresoComunidads
        public ActionResult Index()
        {
            var ingresoComunidad = db.IngresoComunidad.Include(i => i.Comunidad1).Include(i => i.Kermesse1).Include(i => i.Producto1).Include(i => i.Usuario).Include(i => i.Usuario1).Include(i => i.Usuario2);
            return View(ingresoComunidad.ToList());
        }

        // GET: IngresoComunidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Create
        public ActionResult Create()
        {
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre");
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: IngresoComunidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idIngresoComunidad,kermesse,comunidad,producto,cantProducto,totalBonos,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] IngresoComunidad ingresoComunidad)
        {
            if (ModelState.IsValid)
            {
                db.IngresoComunidad.Add(ingresoComunidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // POST: IngresoComunidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idIngresoComunidad,kermesse,comunidad,producto,cantProducto,totalBonos,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] IngresoComunidad ingresoComunidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingresoComunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidad);
        }

        // POST: IngresoComunidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            db.IngresoComunidad.Remove(ingresoComunidad);
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
