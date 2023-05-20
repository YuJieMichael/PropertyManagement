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
    public class PackingController : Controller
    {
        private wyEntities db = new wyEntities();

        public ActionResult Index(string keyword = "")
        {
            var w_packing = db.w_packing.Include(w => w.w_user).Where(p=>p.packing_name.Contains(keyword) || p.packing_lot.Contains(keyword));
            return View(w_packing.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_packing w_packing = db.w_packing.Find(id);
            if (w_packing == null)
            {
                return HttpNotFound();
            }
            return View(w_packing);
        }

        // GET: w_packing/Create
        public ActionResult Create()
        {
            ViewBag.packing_uid = new SelectList(db.w_user, "id", "user_name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,packing_name,packing_lot,packing_lotID,packing_state,packing_type,packing_area,packing_uid")] w_packing w_packing)
        {
            if (ModelState.IsValid)
            {
                db.w_packing.Add(w_packing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.packing_uid = new SelectList(db.w_user, "id", "user_name", w_packing.packing_uid);
            return View(w_packing);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_packing w_packing = db.w_packing.Find(id);
            if (w_packing == null)
            {
                return HttpNotFound();
            }
            ViewBag.packing_uid = new SelectList(db.w_user, "id", "user_name", w_packing.packing_uid);
            return View(w_packing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,packing_name,packing_lot,packing_lotID,packing_state,packing_type,packing_area,packing_uid")] w_packing w_packing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_packing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.packing_uid = new SelectList(db.w_user, "id", "user_name", w_packing.packing_uid);
            return View(w_packing);
        }
        public ActionResult Delete(int? id)
        {
            w_packing w_packing = db.w_packing.Find(id);
            db.w_packing.Remove(w_packing);
            int result = db.SaveChanges();
            if(result > 0)
            {
                return Content("<script>alert('Deleted successfully！');window.location.href='/Packing/Index';</script>");
            }
            else
            {
                return Content("<script>alert('Failed to delete！');window.location.href='/Packing/Index';</script>");
            }
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
