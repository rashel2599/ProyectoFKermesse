using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using ProyectoFinalKermesse.Models;

namespace ProyectoFinalKermesse.Controllers
{
    public class UsuariosController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Usuarios
        public ActionResult Index(string valorB = "")
        {
            var user = from us in db.Usuario select us;
            user = user.Where(us => us.estado.Equals(2) || us.estado.Equals(1));

            if (!string.IsNullOrEmpty(valorB))
            {
                user = user.Where(us => us.nombres.Contains(valorB));
            }


            return View(user.ToList());
        }

        //Get: VerReportes

        public ActionResult VerReporteUsuario(string tipo, string valorB = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptUsuario.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <OutputFormat>EMF</OutputFormat>
                      <PageWidth>8.5in</PageWidth>
                      <PageHeight>11in</PageHeight>
                      <MarginTop>0.25in</MarginTop>
                      <MarginLeft>0.25in</MarginLeft>
                      <MarginRight>0.25in</MarginRight>
                      <EmbedFonts>None</EmbedFonts>
                      <MarginBottom>0.25in</MarginBottom>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var user = from us in db.Usuario select us;
            user = user.Where(us => us.estado.Equals(2) || us.estado.Equals(1));

            if (!string.IsNullOrEmpty(valorB))
            {
                user = user.Where(us => us.nombres.Contains(valorB));
            }

            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Usuario> listaUsuario = new List<Usuario>();
            listaUsuario =  user.ToList();

            ReportDataSource rds = new ReportDataSource("DsUsuario", listaUsuario);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }

        //Get: VerReportesDetalle

        public ActionResult VerReporteUsuarioDetalle(int id)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var user = from us in db.Usuario select us;
            user = user.Where(us => us.idUsuario.Equals(id));

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptUsuarioDetalle.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <OutputFormat>EMF</OutputFormat>
                      <PageWidth>8.5in</PageWidth>
                      <PageHeight>11in</PageHeight>
                      <MarginTop>0.25in</MarginTop>
                      <MarginLeft>0.25in</MarginLeft>
                      <MarginRight>0.25in</MarginRight>
                      <EmbedFonts>None</EmbedFonts>
                      <MarginBottom>0.25in</MarginBottom>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

         

            BDKermesseEntities modelo = new BDKermesseEntities();

            List<Usuario> listaUsuario = new List<Usuario>();
            listaUsuario = modelo.Usuario.ToList();

            ReportDataSource rds = new ReportDataSource("DsUsuario", user.ToList() );
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);


        }



        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var us = new Usuario();
                us.idUsuario = usuario.idUsuario;
                us.userName = usuario.userName;
                us.pwd = usuario.pwd;
                us.nombres = usuario.nombres;
                us.apellidos = usuario.apellidos;
                us.email = usuario.email;
                us.estado = 1;


                db.Usuario.Add(us);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var us = new Usuario();
                us.idUsuario = usuario.idUsuario;
                us.userName = usuario.userName;
                us.pwd = usuario.pwd;
                us.nombres = usuario.nombres;
                us.apellidos = usuario.apellidos;
                us.email = usuario.email;
                us.estado = 2;

                db.Entry(us).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            usuario.estado = 3;

            db.Usuario.Remove(usuario);
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
