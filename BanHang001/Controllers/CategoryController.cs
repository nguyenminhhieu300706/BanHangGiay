using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanHang001.Models;

namespace BanHang001.Controllers
{
    public class CategoryController : Controller
    {
        //khởi tạo database
        MyStoreEntities1 db = new MyStoreEntities1();
        // GET: Category
        public ActionResult DanhSachDanhMuc()
        {
            return View(db.Categories.ToList());
        }
        [HttpGet]
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Them(Category cate)
        {
            db.Categories.Add(cate);
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }
        public ActionResult Sua(string id)
        {
            var cate = db.Categories.Find(id);
            return View(cate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sua(string id, Category cate)
        {
            var cate2 = db.Categories.Find(id);
            cate2.NameCate = cate.NameCate;
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }


        public ActionResult Xoa(string id)
        {
            var cate = db.Categories.Find(id);
            return View(cate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xoa(string id, Category cate)
        {
            cate = db.Categories.Find(id.TrimEnd());
            db.Categories.Remove(cate);
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }

        public ActionResult Chitiet(string id)
        {
            var cate = db.Categories.Find(id);
            return View(cate);
        }
    }
}