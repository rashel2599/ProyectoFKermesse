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
    public class ArqueoCajaDetsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ArqueoCajaDets
        public ActionResult Index(string valorBusq = "")
        {
            var arqueoCajaDet = from ac in db.ArqueoCajaDet select ac; 
            
            arqueoCajaDet = db.ArqueoCajaDet.Include(a => a.ArqueoCaja1).Include(a => a.Denominacion1).Include(a => a.Moneda1);

            if (!string.IsNullOrEmpty(valorBusq))
            {
                arqueoCajaDet = arqueoCajaDet.Where(a => a.Moneda1.nombre.Contains(valorBusq));
            }
            
            
            return View(arqueoCajaDet.ToList());
        }

        // GET: ArqueoCajaDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Create
        public ActionResult Create()
        {
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja");
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras");
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre");
            return View();
        }

        // POST: ArqueoCajaDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idArqueoCajaDet,arqueoCaja,moneda,denominacion,cantidad,subtotal")] ArqueoCajaDet arqueoCajaDet)
        {
            if (ModelState.IsValid)
            {
                db.ArqueoCajaDet.Add(arqueoCajaDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // POST: ArqueoCajaDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idArqueoCajaDet,arqueoCaja,moneda,denominacion,cantidad,subtotal")] ArqueoCajaDet arqueoCajaDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arqueoCajaDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCajaDet);
        }

        // POST: ArqueoCajaDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            db.ArqueoCajaDet.Remove(arqueoCajaDet);
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
