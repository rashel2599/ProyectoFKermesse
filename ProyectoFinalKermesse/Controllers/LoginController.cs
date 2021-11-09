using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProyectoFinalKermesse.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult CloseSesion()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(string User, string Pwd)
        {
            try
            {
                using (Models.BDKermesseEntities db = new Models.BDKermesseEntities())
                {
                    var user = (from d in db.Usuario
                                 where d.userName == User.Trim() && d.pwd == Pwd.Trim()
                                 select d).FirstOrDefault();
                    if (user == null)
                    {
                        ViewBag.Error = "Usuario o contraseña invalida";
                        return View();
                    }

                    Session["User"] = user;
                    Session["Usuario"] = user.userName;


                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }
    }
}