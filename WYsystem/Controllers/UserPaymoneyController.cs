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
    public class UserPaymoneyController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: UserPaymoney
        public ActionResult Index()
        {
            var w_user_paymoney = db.w_user_paymoney.Include(w => w.w_admin).Include(w => w.w_house);
            return View(w_user_paymoney.ToList());
        }

        // GET: UserPaymoney/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_user_paymoney w_user_paymoney = db.w_user_paymoney.Find(id);
            if (w_user_paymoney == null)
            {
                return HttpNotFound();
            }
            return View(w_user_paymoney);
        }

        // GET: UserPaymoney/Create
        public ActionResult Create()
        {
            ViewBag.by_id = new SelectList(db.w_admin, "id", "username");
            ViewBag.house_id = new SelectList(db.w_house, "id", "title");
            return View();
        }

        // POST: UserPaymoney/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,house_id,number,price,should_pay,realy_pay,no_pay,start_pay_time,by_id,title")] w_user_paymoney w_user_paymoney)
        {
            if (ModelState.IsValid)
            {
                db.w_user_paymoney.Add(w_user_paymoney);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.by_id = new SelectList(db.w_admin, "id", "username", w_user_paymoney.by_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_user_paymoney.house_id);
            return View(w_user_paymoney);
        }

        // GET: UserPaymoney/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_user_paymoney w_user_paymoney = db.w_user_paymoney.Find(id);
            if (w_user_paymoney == null)
            {
                return HttpNotFound();
            }
            ViewBag.by_id = new SelectList(db.w_admin, "id", "username", w_user_paymoney.by_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_user_paymoney.house_id);
            return View(w_user_paymoney);
        }

        // POST: UserPaymoney/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,house_id,number,price,should_pay,realy_pay,no_pay,start_pay_time,by_id,title")] w_user_paymoney w_user_paymoney)
        {
            if (ModelState.IsValid)
            {
                db.Entry(w_user_paymoney).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.by_id = new SelectList(db.w_admin, "id", "username", w_user_paymoney.by_id);
            ViewBag.house_id = new SelectList(db.w_house, "id", "title", w_user_paymoney.house_id);
            return View(w_user_paymoney);
        }

        //结清全部货款
        public ActionResult Edit2(int id)
        {
            w_user_paymoney user = db.w_user_paymoney.FirstOrDefault(p=>p.id == id);
            if(user.should_pay == user.realy_pay)
            {
                user.no_pay = 0;
                db.SaveChanges();
                return Content("<script>alert('本费用已结清，无需再次操作！');location.href='/UserPaymoney/Index';</script>");
            }
            //修改用户的金额
            user.no_pay = 0;
            user.realy_pay = user.should_pay; //同步两个的金额
            db.SaveChanges();
            return Content("<script>alert('本费用已结清！');location.href='/UserPaymoney/Index';</script>");
        }

        public ActionResult Delete(int id)
        {
            w_user_paymoney w_user_paymoney = db.w_user_paymoney.Find(id);
            db.w_user_paymoney.Remove(w_user_paymoney);
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
