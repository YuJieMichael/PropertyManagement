using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WYsystem.Filter;
using WYsystem.Models;

namespace WYsystem.Controllers
{
    [AdminAuthen]
    public class PropertyUserController : Controller
    {
        private wyEntities db = new wyEntities();

        // GET: PropertyUser
        public ActionResult Index()
        {
            return View(db.w_property_user.ToList());
        }

        // GET: PropertyUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_property_user w_property_user = db.w_property_user.Find(id);
            if (w_property_user == null)
            {
                return HttpNotFound();
            }
            return View(w_property_user);
        }

        // GET: PropertyUser/Create
        public ActionResult Create()
        {
            return View();
        }

        //需要注意pic这个名称要跟前端的<input type="file" name="pic">的name对应上
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase pic,[Bind(Include = "id,wy_number,name,sex,work_name,id_number,address,phone,work_info")] w_property_user w_property_user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (pic != null)
                    {
                        if (pic.ContentLength == 0) {
                            return Content("<script>alert('Please upload pictures！');location.href='/PropertyUser/Create';</script>");
                        }
                        else
                        {
                            //判断文件的后缀名，是否符合条件
                            string backFix = Path.GetExtension(pic.FileName);
                            if (backFix != ".gif" && backFix != ".png" && backFix != ".jpg" && backFix != ".jpeg")
                            {
                                return Content("<script>alert('Upload image format error！');location.href='/PropertyUser/Create';</script>");
                            }
                            string fileName = DateTime.Now.ToString("MMddHHmmss") + backFix;
                            string strPath = Server.MapPath("~/Content/Upload/" + fileName);
                            pic.SaveAs(strPath);
                            w_property_user.pic = "/Content/Upload/"+ fileName;
                            db.w_property_user.Add(w_property_user);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        return Content("<script>alert('Please select a picture！');location.href='/PropertyUser/Create';</script>");
                    }
                }
                catch (Exception ex)
                {
                    return Content("<script>alert('Upload image exception！');location.href='/PropertyUser/Create';</script>");
                }
            }

            return View(w_property_user);
        }

        // GET: PropertyUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            w_property_user w_property_user = db.w_property_user.Find(id);
            if (w_property_user == null)
            {
                return HttpNotFound();
            }
            return View(w_property_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,wy_number,name,sex,work_name,id_number,address,phone,pic,work_info")] w_property_user w_property_user,string pic2,HttpPostedFileBase pic)
        {
            if (ModelState.IsValid)
            {
                try { 
                    //判断是否有图片需要替换，如果没有则默认使用pic2
                    if (pic != null)
                    {
                        if (pic.ContentLength == 0)
                        {
                            return Content("<script>alert('请上传图片！');location.href='/PropertyUser/Index';</script>");
                        }
                        else
                        {
                            //判断文件的后缀名，是否符合条件
                            string backFix = Path.GetExtension(pic.FileName);
                            if (backFix != ".gif" && backFix != ".png" && backFix != ".jpg" && backFix != ".jpeg")
                            {
                                return Content("<script>alert('Upload image format error！');location.href='/PropertyUser/Index';</script>");
                            }
                            string fileName = DateTime.Now.ToString("MMddHHmmss") + backFix;
                            string strPath = Server.MapPath("~/Content/Upload/" + fileName);
                            pic.SaveAs(strPath);
                            w_property_user.pic = "/Content/Upload/" + fileName;
                        }
                     }
                    else
                    {
                        //如果没有选择图片，就用原来的图片上传
                        w_property_user.pic = pic2;
                    }
                }
                catch (Exception ex)
                {
                    return Content("<script>alert('上传图片异常！');</script>");
                }

                db.Entry(w_property_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(w_property_user);
        }

        public ActionResult Delete(int id)
        {
            w_property_user w_property_user = db.w_property_user.Find(id);
            db.w_property_user.Remove(w_property_user);
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
