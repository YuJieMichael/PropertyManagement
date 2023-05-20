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
    public class BuildingController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: Building
        public ActionResult Index()
        {
            var w_building = db.w_building.Include(w => w.w_system_params);
            return View(w_building.ToList());
        }

        // GET: Building/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_building w_building = db.w_building.Find(id);
            if (w_building == null)
            {
                return HttpNotFound();
            }
            return View(w_building);
        }

        // GET: Building/Create
        public ActionResult Create()
        {
            ViewBag.sp_id = new SelectList(db.w_system_params.Where(p => p.type == "楼宇信息"), "id", "name");
            return View();
        }

        // POST: Building/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,room_name,floors,height,area,createtime,sp_id,remark")] w_building w_building)
        {
            if (ModelState.IsValid)
            {
                db.w_building.Add(w_building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sp_id = new SelectList(db.w_system_params.Where(p => p.type == "楼宇信息"), "id", "name", w_building.sp_id);
            return View(w_building);
        }

        // GET: Building/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_building w_building = db.w_building.Find(id);
            if (w_building == null)
            {
                return HttpNotFound();
            }
            ViewBag.sp_id = new SelectList(db.w_system_params.Where(p => p.type == "楼宇信息"), "id", "name", w_building.sp_id);
            return View(w_building);
        }

        // POST: Building/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,room_name,floors,height,area,createtime,sp_id,remark")] w_building w_building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sp_id = new SelectList(db.w_system_params, "id", "code", w_building.sp_id);
            return View(w_building);
        }


        public ActionResult Delete(int id)
        {
            w_building w_building = db.w_building.Find(id);
            db.w_building.Remove(w_building);
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
