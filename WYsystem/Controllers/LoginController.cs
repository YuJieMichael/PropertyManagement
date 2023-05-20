using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYsystem.Models;
namespace WYsystem.Controllers
{
    public class LoginController : Controller
    {
        private wyEntities db = new wyEntities(); 
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(String username,String password) 
        {
            if (String.IsNullOrEmpty(username))
            {
                ViewBag.notice = "Username cannot be empty！";
                return View();
            }
            if (String.IsNullOrEmpty(password))
            {
                ViewBag.notice = "Password cannot be empty！";
                return View();
            }
                //查询数据库是否存在该用户
                w_admin admin = db.w_admin.FirstOrDefault(p => p.username == username);
            if (admin == null)
            {
                ViewBag.notice = "User does not exist！";
            }
            else if (admin.pass != password)
            {
                ViewBag.notice = "Password error！";
            }else if (admin.power != 1 && admin.power != 3)
            {
                ViewBag.notice = "Your account does not have permission to operate the back office！";
            }
            else 
            {
                //用会话管理记住登陆成功用户信息
                Session["id"] = admin.id;
                Session["power"] = admin.power;
                Session["username"] = admin.username;
                Session["nickname"] = admin.nickname;
                //登陆成功跳转到后台管理界面
                return Redirect("/Admin/index");
            }
            return View();
        }
        public ActionResult logout()
        {
            Session["username"] = null;
            Session["nickname"] = null;
            Session["id"] = null;
            Session["power"] = null;
            return Redirect("/Home/Index");
        }
    }
}