using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WYsystem.Models;
using WYsystem.Filter;

namespace WYsystem.Controllers
{
    [AdminAuthen]
    public class AnnouncementController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: Announcement
        public ActionResult Index(String keyword="")
        {
            var w_announcement = db.w_announcement.Include(w => w.w_admin).Where(p=>p.title.Contains(keyword));
            return View(w_announcement.ToList());
        }

        // GET: Announcement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_announcement w_announcement = db.w_announcement.Find(id);
            if (w_announcement == null)
            {
                return HttpNotFound();
            }
            return View(w_announcement);
        }

        // GET: Announcement/Create
        public ActionResult Create()
        {
            ViewBag.uid = new SelectList(db.w_admin, "id", "username");
            return View();
        }

        // POST: Announcement/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,number,title,createtime,contents,uid,nickname")] w_announcement w_announcement)
        {
            if (ModelState.IsValid)
            {
                db.w_announcement.Add(w_announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.uid = new SelectList(db.w_admin, "id", "username", w_announcement.uid);
            return View(w_announcement);
        }

        // GET: Announcement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_announcement w_announcement = db.w_announcement.Find(id);
            if (w_announcement == null)
            {
                return HttpNotFound();
            }
            ViewBag.uid = new SelectList(db.w_admin, "id", "username", w_announcement.uid);
            return View(w_announcement);
        }

        // POST: Announcement/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,number,title,createtime,contents,uid,nickname")] w_announcement w_announcement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.uid = new SelectList(db.w_admin, "id", "username", w_announcement.uid);
            return View(w_announcement);
        }

        // GET: Announcement/Delete/5
        public ActionResult Delete(int id)
        {
            w_announcement w_announcement = db.w_announcement.Find(id);
            db.w_announcement.Remove(w_announcement);
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
