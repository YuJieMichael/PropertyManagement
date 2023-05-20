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
    public class InstallationController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: Installation
        public ActionResult Index(String keyword="")
        {
            var w_installation = db.w_installation.Include(w => w.w_system_params).Where(p=>p.title.Contains(keyword));
            return View(w_installation.ToList());
        }

        // GET: Installation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_installation w_installation = db.w_installation.Find(id);
            if (w_installation == null)
            {
                return HttpNotFound();
            }
            return View(w_installation);
        }

        // GET: Installation/Create
        public ActionResult Create()
        {
            ViewBag.sp_id = new SelectList(db.w_system_params.Where(p => p.type == "周边设施"), "id", "name");
            return View();
        }

        // POST: Installation/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,sp_id,title,name,phone,main_name,contents")] w_installation w_installation)
        {
            if (ModelState.IsValid)
            {
                db.w_installation.Add(w_installation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sp_id = new SelectList(db.w_system_params.Where(p => p.type == "周边设施"), "id", "name", w_installation.sp_id);
            return View(w_installation);
        }

        // GET: Installation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_installation w_installation = db.w_installation.Find(id);
            if (w_installation == null)
            {
                return HttpNotFound();
            }
            ViewBag.sp_id = new SelectList(db.w_system_params.Where(p => p.type == "周边设施"), "id", "name", w_installation.sp_id);
            return View(w_installation);
        }

        // POST: Installation/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,sp_id,name,phone,main_name,contents")] w_installation w_installation)
        {
            
            //if (ModelState.IsValid)
            //{
                db.Entry(w_installation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
           // }
           // ViewBag.sp_id = new SelectList(db.w_system_params, "id", "code", w_installation.sp_id);
           // return View(w_installation);
        }

        public ActionResult Delete(int id)
        {
            w_installation w_installation = db.w_installation.Find(id);
            db.w_installation.Remove(w_installation);
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
