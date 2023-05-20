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
    public class WadminController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: Wadmin
        public ActionResult Index()
        {
            return View(db.w_admin.Where(p => p.power == 1 || p.power == 2).ToList());
        }

        //用户首页
        public ActionResult UserIndex()
        {
            return View(db.w_admin.Where(p=>p.power == 0).ToList());
        }

        //用户编辑模块
        public ActionResult UserEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_admin w_admin = db.w_admin.Find(id);
            if (w_admin == null)
            {
                return HttpNotFound();
            }
            return View(w_admin);
        }

        //以get方式提交删除数据
        public ActionResult UserDelete(int id)
        {
            w_admin w_admin = db.w_admin.Find(id);
            db.w_admin.Remove(w_admin);
            db.SaveChanges();
            return RedirectToAction("UserIndex");
        }

        // GET: Wadmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_admin w_admin = db.w_admin.Find(id);
            if (w_admin == null)
            {
                return HttpNotFound();
            }
            return View(w_admin);
        }

        // GET: Wadmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wadmin/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性。有关
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,username,pass,nickname,power,createtime")] w_admin w_admin)
        {
            if (ModelState.IsValid)
            {
                db.w_admin.Add(w_admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(w_admin);
        }

        // GET: Wadmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_admin w_admin = db.w_admin.Find(id);
            if (w_admin == null)
            {
                return HttpNotFound();
            }
            return View(w_admin);
        }

        // POST: Wadmin/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性。有关
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,username,pass,nickname,power,createtime")] w_admin w_admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(w_admin);
        }

        [HttpPost]
        public ActionResult UserEdits([Bind(Include = "id,username,pass,nickname,power,createtime")] w_admin w_admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserIndex");
            }
            return View(w_admin);
        }

        // GET: Wadmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_admin w_admin = db.w_admin.Find(id);
            if (w_admin == null)
            {
                return HttpNotFound();
            }
            return View(w_admin);
        }
        //以get方式提交删除数据
        public ActionResult DeleteConfirmed(int id)
        {
            w_admin w_admin = db.w_admin.Find(id);
            db.w_admin.Remove(w_admin);
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
