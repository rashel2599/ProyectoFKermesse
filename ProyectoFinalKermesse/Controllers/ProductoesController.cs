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
    public class ProductoesController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Productoes
        public ActionResult Index(String valorBusq= "")
        {
            var producto = from p in db.Producto select p;

            producto = db.Producto.Include(p => p.CategoriaProducto).Include(p => p.Comunidad1);
            producto = producto.Where(p => p.estado.Equals(1) || p.estado.Equals(2));


            if (!string.IsNullOrEmpty(valorBusq))
            {
                producto = producto.Where(p => p.nombre.Contains(valorBusq));
            }


            return View(producto.ToList());
        }

        // GET: Productoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productoes/Create
        public ActionResult Create()
        {
            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre");
            return View();
        }

        // POST: Productoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var p = new Producto();

                p.nombre = producto.nombre;
                p.comunidad = producto.comunidad;
                p.descripcion = producto.descripcion;
                p.catProd = producto.catProd;
                p.precioVSugerido = producto.precioVSugerido;
                p.cantidad = producto.cantidad;
                p.estado = 1;

                db.Producto.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre", producto.catProd);
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", producto.comunidad);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre", producto.catProd);
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", producto.comunidad);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProducto,comunidad,catProd,nombre,descripcion,cantidad,precioVSugerido,estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.estado = 2;


                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre", producto.catProd);
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", producto.comunidad);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Producto producto = db.Producto.Find(id);
                db.Producto.Remove(producto);
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
