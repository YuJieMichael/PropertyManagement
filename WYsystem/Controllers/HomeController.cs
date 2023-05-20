using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYsystem.Models;
using System.Data.Entity;

namespace WYsystem.Controllers
{
    public class HomeController : Controller
    {
        private wyEntities db = new wyEntities();
        //物业系统官网首页
        public ActionResult Index()
        {
            //【联系物业】小模块信息
            ViewBag.ContactWuye = db.w_property_user.Where(w => w.wy_number == "666").ToList();

            //公告置顶预览（置顶）
            ViewBag.announcement = db.w_announcement.Where(a => a.uid == 1).Take(1).ToList();

            //周边设施预览（置顶）
            ViewBag.zbInfo = db.w_installation.Where(z => z.id == 1).Include(w => w.w_system_params).Take(1).ToList();

            //物业人员预览（置顶）
            ViewBag.proUser = db.w_property_user.OrderBy(p => Guid.NewGuid()).Take(1).ToList(); //这里是从物业人员表中随机取出一条

            //小区信息预览
            ViewBag.buildingInfo = db.w_building.Include(s => s.w_system_params).OrderBy(p => Guid.NewGuid()).Take(1).ToList();//随机取一条

            return View();
        }
        [HttpPost]
        //登录请求方法（重写Index形成一个新方法）
        public ActionResult Index(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                ViewBag.notice = "账号不能为空！";
            }
            if (string.IsNullOrEmpty(password))
            {
                ViewBag.notice = "密码不能为空！";
            }
            w_user user = db.w_user.FirstOrDefault(u => u.username == username);
            //1、先判断用户是否存在，不能够先判断密码是否正确，一定是先找到这个用户，才进行密码判断
            if (user == null)
            {
                return Content("UserNull");
            }
            //2、user存在就会再进行判断，这时候就可以判断密码是否正确
            else if (user.password != password)
            {
                return Content("PassError");
            }
            else
            {
                //会话窗口保存业主信息
                Session["User"] = user;
                return Content("OK");
            }
        }

        //物业公告页面
        public ActionResult WyNotice(string keywords = "")
        {
            //置顶公告显示
            ViewBag.TopNotice = db.w_announcement.Where(a => a.uid == 1 && a.title.Contains(keywords)).Take(1).ToList();

            //全部公告显示（不包括置顶）
            var NoticeInfo = db.w_announcement.Where(a => a.uid != 1 && a.title.Contains(keywords)).ToList();//Contains关键字搜索
            return View(NoticeInfo);
        }

        //物业公告详情页
        public ActionResult WyNoticeDetail(int? id)
        {
            //显示详情数据
            var NoticeDetail = db.w_announcement.Where(a => a.id == id).ToList();
            return View(NoticeDetail);
        }

        //周边设施页面
        public ActionResult WyInstallation(string keywords = "")
        {
            //全部设施显示
            var InstallationInfo = db.w_installation.Include(a=>a.w_system_params).Where(a => a.title.Contains(keywords)).ToList();//Contains关键字搜索
            return View(InstallationInfo);
        }

        //周边设施详情页
        public ActionResult WyInstallationDetail(int? id)
        {
            //显示详情数据
            var InstallationDetail = db.w_installation.Where(a => a.id == id).ToList();
            return View(InstallationDetail);
        }

        //物业人员页
        public ActionResult WyProUser()
        {
            var proUserInfo = db.w_property_user.ToList();
            return View(proUserInfo);
        }

        //退出登录操作
        [HttpGet]
        public ActionResult Logout()
        {
            Session["User"] = null;
            return Content("<script>alert('退出成功！');window.location.href='/Home/Index';</script>");
        }
    }
}