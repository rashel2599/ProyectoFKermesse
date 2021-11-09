using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoFinalKermesse.Controllers;
using ProyectoFinalKermesse.Models;


namespace ProyectoFinalKermesse.Filters
{
    public class VerificarSesion : ActionFilterAttribute
    {
        private Usuario user; 
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                base.OnActionExecuted(filterContext);

                user = (Usuario)HttpContext.Current.Session["User"];

                if (user == null)
                {

                    if (filterContext.Controller is LoginController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Login/Login");
                    }

                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Login/Login");
            }

        }

    }
}