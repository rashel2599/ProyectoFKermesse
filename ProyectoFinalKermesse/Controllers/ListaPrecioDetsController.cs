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
    public class ListaPrecioDetsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ListaPrecioDets
        public ActionResult Index()
        {
            var listaPrecioDet = db.ListaPrecioDet.Include(l => l.ListaPrecio1).Include(l => l.Producto1);
            return View(listaPrecioDet.ToList());
        }

        // GET: ListaPrecioDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Create
        public ActionResult Create()
        {
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");
            return View();
        }

        // POST: ListaPrecioDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idListaPrecioDet,listaPrecio,producto,precioVenta")] ListaPrecioDet listaPrecioDet)
        {
            if (ModelState.IsValid)
            {
                db.ListaPrecioDet.Add(listaPrecioDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // POST: ListaPrecioDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idListaPrecioDet,listaPrecio,producto,precioVenta")] ListaPrecioDet listaPrecioDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listaPrecioDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecioDet);
        }

        // POST: ListaPrecioDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            db.ListaPrecioDet.Remove(listaPrecioDet);
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
