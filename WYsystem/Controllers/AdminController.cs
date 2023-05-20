using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYsystem.Models;

namespace WYsystem.Controllers
{
    public class AdminController : Controller
    {
        private wyEntities db = new wyEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["nickname"] == null)
            {
                return Redirect("/Login/Index");
            }
            return View();
        }
        //小区管理部分
        //小区管理首页
        public ActionResult RoomIndex()
        {
            //获取小区信息回来展示
            w_room_info info = db.w_room_info.FirstOrDefault();
            return View(info);
        }
        //实现添加小区信息
        public ActionResult AddRoom()
        {
            return View();
        }
        //实现添加小区操作
        [HttpPost]
        public ActionResult AddRoom(w_room_info room)
        {
            ViewBag.notice = "";
            db.w_room_info.Add(room);
            //执行修改数据库操作
            int result = db.SaveChanges();
            if(result > 0)
            {
                //保存成功提示，并返回首页
                return Content("<script>alert('Add cell information successfully');window.location.href = '/Admin/RoomIndex';</script>");
            }
            else
            {
                ViewBag.notice = "Failed to save cell information please try again";
            }
            return View();
        }
        //编辑小区信息
        public ActionResult UpdateRoom()
        {
            //取一条信息
            w_room_info info = db.w_room_info.FirstOrDefault();
            if(info == null)
            {
                //如果不存在编辑的信息，需要先提示新增信息
                return Content("<script>alert('The information you need to edit is not found, please add the information first'); window.location.href='/Admin/AddRoom';</script>");
            }
            return View(info);
        }
        [HttpPost]
        public ActionResult UpdateRoom(w_room_info info)
        {
            db.Entry(info).State = System.Data.Entity.EntityState.Modified;
            //保存修改操作
            if(db.SaveChanges() > 0)
            {
                return Content("<script>alert('Edit cell information successfully'); window.location.href = '/Admin/RoomIndex';</script>");
            }
            else
            {
                ViewBag.notice = "Edit cell information failed please try again";
            }
            return View();
        }

    }
}