using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WYsystem.Filter
{
    public class AdminAuthen : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //1、获取管理员登录信息
            string admin_id = filterContext.HttpContext.Session["id"]?.ToString();
            string admin_power = filterContext.HttpContext.Session["power"]?.ToString();
            string admin_username = filterContext.HttpContext.Session["username"]?.ToString();
            string admin_nickname = filterContext.HttpContext.Session["nickname"]?.ToString();

            if (admin_id == null || admin_power == null || admin_username == null || admin_nickname == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
        }
}
}