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
    public class ArqueoCajasController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ArqueoCajas
        public ActionResult Index(string valorBusq = "")
        {
            var arqueoCaja = from ac in db.ArqueoCaja select ac; 
                
            arqueoCaja = db.ArqueoCaja.Include(a => a.Kermesse1).Include(a => a.Usuario).Include(a => a.Usuario1).Include(a => a.Usuario2);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                arqueoCaja = arqueoCaja.Where(a => a.Kermesse1.nombre.Contains(valorBusq));
            }

            return View(arqueoCaja.ToList());
        }

        // GET: ArqueoCajas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Create
        public ActionResult Create()
        {
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: ArqueoCajas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idArqueoCaja,kermesse,fechaArqueo,granTotal,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] ArqueoCaja arqueoCaja)
        {
            if (ModelState.IsValid)
            {
                db.ArqueoCaja.Add(arqueoCaja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // POST: ArqueoCajas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idArqueoCaja,kermesse,fechaArqueo,granTotal,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] ArqueoCaja arqueoCaja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arqueoCaja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCaja);
        }

        // POST: ArqueoCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            db.ArqueoCaja.Remove(arqueoCaja);
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
