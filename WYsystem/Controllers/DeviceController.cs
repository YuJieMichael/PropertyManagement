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
    public class DeviceController : Controller
    {
        private wyEntities db = new wyEntities();

        public ActionResult Index(string keyword = "")
        {
            return View(db.w_device.Where(p => p.device_name.Contains(keyword) || p.device_desc.Contains(keyword)).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_device w_device = db.w_device.Find(id);
            if (w_device == null)
            {
                return HttpNotFound();
            }
            return View(w_device);
        }

        // GET: Device/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,device_id,device_name,device_desc,createtime")] w_device w_device)
        {
            if (ModelState.IsValid)
            {
                db.w_device.Add(w_device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(w_device);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_device w_device = db.w_device.Find(id);
            if (w_device == null)
            {
                return HttpNotFound();
            }
            return View(w_device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,device_id,device_name,device_desc,createtime")] w_device w_device)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(w_device);
        }

        public ActionResult Delete(int? id)
        {
            w_device w_device = db.w_device.Find(id);
            db.w_device.Remove(w_device);
            int result = db.SaveChanges();
            if (result > 0)
            {
                return Content("<script>alert('删除成功！');window.location.href='/Device/Index';</script>");
            }
            else
            {
                return Content("<script>alert('删除失败！');window.location.href='/Device/Index';</script>");
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
