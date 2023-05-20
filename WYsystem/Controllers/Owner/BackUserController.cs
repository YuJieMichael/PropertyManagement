using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WYsystem.Filter;
using WYsystem.Models;
using System.Data.Entity;

namespace WYsystem.Controllers.Owner
{
    public class BackUserController : Controller
    {
        private wyEntities db = new wyEntities();

        // 用户个人后台
        [UserAuthen]
        public ActionResult Index()
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            ViewBag.UserInfo = db.w_user.Where(u => u.id == user.id).ToList();

            //报修记录（取最新3条展示）
            ViewBag.RepairRecord = db.w_repair.Where(r => r.uid == user.id).OrderByDescending(p => p.id).Take(3).ToList();
            return View();
        }

        // 我的个人中心
        [UserAuthen]
        public ActionResult MyCenter()
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            w_user UserInfo = db.w_user.Where(u => u.id == user.id).FirstOrDefault();
            return View(UserInfo);
        }

        //修改登录信息
        [UserAuthen]
        public ActionResult EditMyForm()
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            w_user UserInfo = db.w_user.Where(u => u.id == user.id).FirstOrDefault();
            return View(UserInfo);
        }
        //修改参数方法
        [HttpPost]
        public ActionResult EditMyForm(w_user w_user, string oldPass)
        {
            w_user user = db.w_user.FirstOrDefault(u => u.id == w_user.id);
            if (user.password == oldPass)
            {
                user.username = w_user.username;
                user.password = w_user.password;
                user.phone = w_user.phone;
                user.email = w_user.email;
                user.link_address = w_user.link_address;
                user.work_address = w_user.work_address;
                db.SaveChanges();
                return Content("OK");
            }
            else
            {
                return ViewBag.notice = "旧密码输入不正确";
            }
        }


        [UserAuthen]
        //我的物业收费
        public ActionResult MyCharge(string keyword = "",int page = 1,int pageSize = 5)
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            var Charge = db.w_user_paymoney.Include(p=>p.w_house).Where(p => p.house_id == user.house_id && p.title.Contains(keyword)).ToList();
            ViewBag.ListCount = db.w_user_paymoney.Include(p => p.w_house).Where(p => p.house_id == user.house_id).Count();

            //分页
            //判断页码数是否为负值
            if(page < 1)
            {
                page = 1;
            }
            //判断最大的页码数
            if(page > Math.Ceiling((decimal)Charge.Count / pageSize))
            {
                page = (int)Math.Ceiling((decimal)Charge.Count / page);
            }
            ViewBag.pageCount = Math.Ceiling((decimal)Charge.Count / pageSize); //最大页数
            ViewBag.page = page;//页面索引
            ViewBag.pageSize = pageSize;//每页条数
            Charge = Charge.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(p => p.start_pay_time).ToList();
            return View(Charge);
        }

        //发起报修
        [UserAuthen]
        public ActionResult AddRepair()
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            return View(user);
        }

        //发起报修方法
        [HttpPost]
        public ActionResult AddRepair(w_repair w_Repair)
        {
            w_Repair.createtime.Value.ToString("yyyy-mm-dd");
            w_Repair.repair_number = DateTime.Now.ToString("yyyy-mm-dd");
            w_Repair.state = 0;
            db.w_repair.Add(w_Repair);
            int result = db.SaveChanges();
            if(result == 0)
            {
                return Content("Error");
            }
            else
            {
                return Content("OK");
            }
        }

        //我的报修记录
        [UserAuthen]
        public ActionResult MyRepairRecord(int page = 1,int pageSize = 5)
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            //查询自己发布的报修记录
            var myRepair = db.w_repair.Include(p=>p.w_user).Where(p => p.uid == user.id && p.state == 0).OrderByDescending(p => p.id).ToList();
            ViewBag.MyRecordCount = db.w_repair.Include(p => p.w_user).Where(p => p.uid == user.id && p.state == 0).OrderByDescending(p => p.id).Count();

            //[已发布的报修记录]分页
            //判断页码数是否为负值
            if (page < 1)
            {
                page = 1;
            }
            //判断最大的页码数
            if (page > Math.Ceiling((decimal)myRepair.Count / pageSize))
            {
                page = (int)Math.Ceiling((decimal)myRepair.Count / page);
            }
            ViewBag.pageCount = Math.Ceiling((decimal)myRepair.Count / pageSize); //最大页数
            ViewBag.page = page;//页面索引
            ViewBag.pageSize = pageSize;//每页条数
            ViewBag.MyRecord = myRepair.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(p => p.id).ToList();

            //已审核的记录
            var myRecord = db.w_repair.Include(p => p.w_user).Where(p => p.uid == user.id && p.state == 1).OrderByDescending(p => p.id).ToList();
            ViewBag.RepairRecordCount = db.w_repair.Include(p => p.w_user).Where(p => p.uid == user.id && p.state == 1).OrderByDescending(p => p.id).Count();
            //[审核的记录]分页
            //判断页码数是否为负值
            if (page < 1)
            {
                page = 1;
            }
            //判断最大的页码数
            if (page > Math.Ceiling((decimal)myRecord.Count / pageSize))
            {
                page = (int)Math.Ceiling((decimal)myRecord.Count / page);
            }
            ViewBag.pageCount2 = Math.Ceiling((decimal)myRecord.Count / pageSize); //最大页数
            ViewBag.page2 = page;//页面索引
            ViewBag.pageSize2 = pageSize;//每页条数
            ViewBag.RepairRecord = myRecord.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(p => p.id).ToList();
            return View();
        }

        //发起投诉
        [UserAuthen]
        public ActionResult AddComplaint()
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult AddComplaint(w_complaint w_Complaint)
        {
            w_Complaint.createtime.Value.ToString("yyyy-mm-dd");
            w_Complaint.house_id = (Session["User"] as WYsystem.Models.w_user).house_id;
            db.w_complaint.Add(w_Complaint);
            int result = db.SaveChanges();
            if (result == 0)
            {
                return Content("Error");
            }
            else
            {
                return Content("OK");
            }
        }
        //我的投诉记录
        [UserAuthen]
        public ActionResult MyComplaintRecord()
        {
            //取当前登录之后的用户回来
            var user = Session["User"] as WYsystem.Models.w_user;
            if (user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            //查询自己发布的投诉记录
            ViewBag.MyComplaint = db.w_complaint.Where(p => p.uid == user.id).OrderByDescending(p => p.id).ToList();
            ViewBag.MyComplaintCount = db.w_complaint.Where(p => p.uid == user.id).OrderByDescending(p => p.id).Count();
            //已审核的记录
            ViewBag.ComplaintRecord = db.w_complaint.Where(p => p.uid == user.id && p.state != "未审核").OrderByDescending(p => p.id).ToList();
            ViewBag.ComplaintRecordCount = db.w_complaint.Where(p => p.uid == user.id && p.state != "未审核").OrderByDescending(p => p.id).Count();
            return View();
        }
    }
}