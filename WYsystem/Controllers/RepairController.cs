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
    public class RepairController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: Repair
        public ActionResult Index()
        {
            var w_repair = db.w_repair.Include(w => w.w_building).Include(w => w.w_house).Include(w => w.w_system_params).Include(w => w.w_user);
            return View(w_repair.ToList());
        }

        // GET: Repair/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_repair w_repair = db.w_repair.Find(id);
            if (w_repair == null)
            {
                return HttpNotFound();
            }
            return View(w_repair);
        }

        // GET: Repair/Create
        public ActionResult Create()
        {
            ViewBag.louyu_id = new SelectList(db.w_building, "id", "room_name");
            ViewBag.house_id = new SelectList(db.w_house, "id", "title");
            ViewBag.danyuan_id = new SelectList(db.w_system_params, "id", "code");
            ViewBag.uid = new SelectList(db.w_user, "id", "user_name");
            return View();
        }

        // POST: Repair/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,unit_name,uid,describe,state,createtime,repeat_info,state_type,louyu_id,danyuan_id,repair_number,house_id,finaly_repair_user,repair_work_info,main_repair_user,repair_phone,repair_pass,pass_detail,repair_info")] w_repair w_repair)
        {
            if (ModelState.IsValid)
            {
                db.w_repair.Add(w_repair);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.louyu_id = new SelectList(db.w_building, "id", "room_name", w_repair.louyu_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_repair.house_id);
            ViewBag.danyuan_id = new SelectList(db.w_system_params, "id", "code", w_repair.danyuan_id);
            ViewBag.uid = new SelectList(db.w_user, "id", "user_name", w_repair.uid);
            return View(w_repair);
        }

        //处理用户的报修
        public ActionResult Edit2(int id)
        {
            w_repair com = db.w_repair.FirstOrDefault(p => p.id == id);
            if (com.state == 1)
            {
                return Content("<script>alert('本报修已审核！');location.href='/Repair/Index';</script>");
            }
            else
            {
                com.state = 1;
                db.SaveChanges();
                return Content("<script>alert('审核完成！');location.href='/Repair/Index';</script>");
            }
        }

        // GET: Repair/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_repair w_repair = db.w_repair.Find(id);
            if (w_repair == null)
            {
                return HttpNotFound();
            }
            ViewBag.louyu_id = new SelectList(db.w_building, "id", "room_name", w_repair.louyu_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_repair.house_id);
            ViewBag.danyuan_id = new SelectList(db.w_system_params, "id", "code", w_repair.danyuan_id);
            ViewBag.uid = new SelectList(db.w_user, "id", "user_name", w_repair.uid);
            return View(w_repair);
        }

        // POST: Repair/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,unit_name,uid,describe,state,createtime,repeat_info,state_type,louyu_id,danyuan_id,repair_number,house_id,finaly_repair_user,repair_work_info,main_repair_user,repair_phone,repair_pass,pass_detail,repair_info")] w_repair w_repair)
        {
            w_repair com = db.w_repair.FirstOrDefault(p => p.id == w_repair.id);
            if (com.state == 2)
            {
                return Content("<script>alert('本报修已处理过！');location.href='/Repair/Index';</script>");
            }
            else
            {
                com.state = w_repair.state;
                com.finaly_repair_user = w_repair.finaly_repair_user;
                com.repair_work_info = w_repair.repair_work_info;
                com.main_repair_user = w_repair.main_repair_user;
                com.repair_phone = w_repair.repair_phone;
                com.pass_detail = w_repair.pass_detail;
                db.SaveChanges();
                return Content("<script>alert('处理完成！');location.href='/Repair/Index';</script>");
            }
        }

        public ActionResult Delete(int id)
        {
            w_repair w_repair = db.w_repair.Find(id);
            db.w_repair.Remove(w_repair);
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
