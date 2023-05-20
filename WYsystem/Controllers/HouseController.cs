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
    public class HouseController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: House
        public ActionResult Index(String keyword="")
        {
            var w_house = db.w_house.Include(w => w.w_building).Include(w => w.w_system_params).Include(w => w.w_system_params1).Include(w => w.w_system_params2).Include(w => w.w_system_params3).Include(w => w.w_system_params4).Include(w => w.w_system_params5).Where(p=>p.title.Contains(keyword));
            return View(w_house.ToList());
        }

        // GET: House/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_house w_house = db.w_house.Find(id);
            if (w_house == null)
            {
                return HttpNotFound();
            }
            return View(w_house);
        }

        // GET: House/Create
        public ActionResult Create()
        {
            ViewBag.b_id = new SelectList(db.w_building, "id", "room_name");
            ViewBag.d_id = new SelectList(db.w_system_params.Where(p => p.type == "单元信息"), "id", "name");
            ViewBag.d_room = new SelectList(db.w_system_params.Where(p => p.type == "规格信息"), "id", "name");
            ViewBag.c_id = new SelectList(db.w_system_params.Where(p => p.type == "朝向信息"), "id", "name");
            ViewBag.r_id = new SelectList(db.w_system_params.Where(p => p.type == "装修标准"), "id", "name");
            ViewBag.g_id = new SelectList(db.w_system_params.Where(p => p.type == "功能信息"), "id", "name");
            //多的字段，不需要
            ViewBag.bz_id = new SelectList(db.w_system_params.Where(p => p.type == "规格信息"), "id", "name");
            return View();
        }

        // POST: House/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,b_id,d_id,d_room,c_id,r_id,g_id,bz_id,area,use_area,is_use")] w_house w_house)
        {
            if (ModelState.IsValid)
            {
                db.w_house.Add(w_house);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.b_id = new SelectList(db.w_building, "id", "room_name", w_house.b_id);
            ViewBag.d_id = new SelectList(db.w_system_params.Where(p => p.type == "单元信息"), "id", "name", w_house.d_id);
            ViewBag.d_room = new SelectList(db.w_system_params.Where(p => p.type == "规格信息"), "id", "name", w_house.d_room);
            ViewBag.c_id = new SelectList(db.w_system_params.Where(p => p.type == "朝向信息"), "id", "name", w_house.c_id);
            ViewBag.r_id = new SelectList(db.w_system_params.Where(p => p.type == "装修标准"), "id", "name", w_house.r_id);
            ViewBag.g_id = new SelectList(db.w_system_params.Where(p => p.type == "规格信息"), "id", "name", w_house.g_id);
            ViewBag.bz_id = new SelectList(db.w_system_params, "id", "name", w_house.bz_id);
            return View(w_house);
        }

        // GET: House/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_house w_house = db.w_house.Find(id);
            if (w_house == null)
            {
                return HttpNotFound();
            }
            ViewBag.d_id = new SelectList(db.w_system_params.Where(p => p.type == "单元信息"), "id", "name", w_house.d_id);
            ViewBag.d_room = new SelectList(db.w_system_params.Where(p => p.type == "规格信息"), "id", "name", w_house.d_room);
            ViewBag.c_id = new SelectList(db.w_system_params.Where(p => p.type == "朝向信息"), "id", "name", w_house.c_id);
            ViewBag.r_id = new SelectList(db.w_system_params.Where(p => p.type == "装修标准"), "id", "name", w_house.r_id);
            ViewBag.g_id = new SelectList(db.w_system_params.Where(p => p.type == "功能信息"), "id", "name", w_house.g_id);
            ViewBag.bz_id = new SelectList(db.w_system_params, "id", "code", w_house.bz_id);
            return View(w_house);
        }

        // POST: House/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,b_id,d_id,d_room,c_id,r_id,g_id,bz_id,area,use_area,is_use")] w_house w_house)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_house).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.b_id = new SelectList(db.w_building, "id", "room_name", w_house.b_id);
            ViewBag.d_id = new SelectList(db.w_system_params, "id", "code", w_house.d_id);
            ViewBag.d_room = new SelectList(db.w_system_params, "id", "code", w_house.d_room);
            ViewBag.c_id = new SelectList(db.w_system_params, "id", "code", w_house.c_id);
            ViewBag.r_id = new SelectList(db.w_system_params, "id", "code", w_house.r_id);
            ViewBag.g_id = new SelectList(db.w_system_params, "id", "code", w_house.g_id);
            return View(w_house);
        }


        // POST: House/Delete/5
        public ActionResult Delete(int id)
        {
            w_house w_house = db.w_house.Find(id);
            db.w_house.Remove(w_house);
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
