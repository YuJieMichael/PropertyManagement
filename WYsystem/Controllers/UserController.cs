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
    public class UserController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: User
        public ActionResult Index()
        {
            var w_user = db.w_user.Include(w => w.w_building).Include(w => w.w_house).Include(w => w.w_system_params);
            return View(w_user.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_user w_user = db.w_user.Find(id);
            if (w_user == null)
            {
                return HttpNotFound();
            }
            return View(w_user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.building_id = new SelectList(db.w_building, "id", "room_name");
            ViewBag.house_id = new SelectList(db.w_house, "id", "title");
            ViewBag.danyuan_id = new SelectList(db.w_system_params.Where(p => p.type == "单元信息"), "id", "name");
            return View();
        }

        // POST: User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,building_id,danyuan_id,house_id,user_name,house_number,phone,email,id_number,work_address,link_address,username,password,remark,createtime")] w_user w_user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.w_user.Add(w_user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }catch(Exception e)
                {
                    return Content("<script>alert('添加的用户名已存在！');location.href='/User/Create';</script>");
                }
            }

            ViewBag.building_id = new SelectList(db.w_building, "id", "room_name", w_user.building_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_user.house_id);
            ViewBag.danyuan_id = new SelectList(db.w_system_params, "id", "code", w_user.danyuan_id);
            return View(w_user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_user w_user = db.w_user.Find(id);
            if (w_user == null)
            {
                return HttpNotFound();
            }
            ViewBag.building_id = new SelectList(db.w_building, "id", "room_name", w_user.building_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_user.house_id);
            ViewBag.danyuan_id = new SelectList(db.w_system_params, "id", "code", w_user.danyuan_id);
            return View(w_user);
        }

        // POST: User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,building_id,danyuan_id,house_id,user_name,house_number,phone,email,id_number,work_address,link_address,username,password,remark,createtime")] w_user w_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.building_id = new SelectList(db.w_building, "id", "room_name", w_user.building_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_user.house_id);
            ViewBag.danyuan_id = new SelectList(db.w_system_params, "id", "code", w_user.danyuan_id);
            return View(w_user);
        }

        public ActionResult Delete(int id)
        {
            w_user w_user = db.w_user.Find(id);
            db.w_user.Remove(w_user);
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
