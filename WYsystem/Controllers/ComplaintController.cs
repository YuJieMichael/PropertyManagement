using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WYsystem.Filter;
using WYsystem.Models;

namespace WYsystem.Controllers
{
    [AdminAuthen]
    public class ComplaintController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: Complaint
        public ActionResult Index()
        {
            return View(db.w_complaint.Include(p=>p.w_house).ToList());
        }

        // GET: Complaint/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_complaint w_complaint = db.w_complaint.Find(id);
            if (w_complaint == null)
            {
                return HttpNotFound();
            }
            return View(w_complaint);
        }

        // GET: Complaint/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Complaint/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,uid,address,phone,describe,state,createtime,title,is_use,result,house_id,pass_detail")] w_complaint w_complaint)
        {
            if (ModelState.IsValid)
            {
                db.w_complaint.Add(w_complaint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(w_complaint);
        }

        // GET: Complaint/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_complaint w_complaint = db.w_complaint.Find(id);
            if (w_complaint == null)
            {
                return HttpNotFound();
            }
            return View(w_complaint);
        }

        // POST: Complaint/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string result,string pass_detail,string state,int id)
        {
            w_complaint w_complaint = db.w_complaint.FirstOrDefault(p => p.id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    w_complaint.result = result;
                    w_complaint.pass_detail = pass_detail;
                    w_complaint.state = state;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    return Content("<script>alert('电话号码格式不正确！');location.href='/Complaint/Index';</script>");
                }
            }

            return View(w_complaint);
        }

        public ActionResult Edit2(int id)
        {
            w_complaint com = db.w_complaint.FirstOrDefault(p=>p.id == id);
            if(com.state != "未审核")
            {
                return Content("<script>alert('本投诉已不用审核！');location.href='/Complaint/Index';</script>");
            }
            else
            {
                try
                {
                    com.is_use = 2;
                    com.state = "已审核";
                    db.SaveChanges();
                    return Content("<script>alert('审核完成！');location.href='/Complaint/Index';</script>");
                }
                catch(Exception e)
                {
                    return Content("<script>alert('电话号码格式不正确！');location.href='/Complaint/Index';</script>");
                }
                
            }
        }

        public ActionResult Delete(int id)
        {
            w_complaint w_complaint = db.w_complaint.Find(id);
            db.w_complaint.Remove(w_complaint);
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
