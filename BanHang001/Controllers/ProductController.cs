using BanHang001.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanHang001.Controllers;  

namespace BanHang001.Controllers
{
    public class ProductController : Controller
    {
        MyStoreEntities1 db = new MyStoreEntities1();
        // GET: Product
        public ActionResult IndexCus(decimal? _min, decimal? _max)
        {
            var profilter = db.Products.AsQueryable();

            if (_min.HasValue)
                profilter = profilter.Where(s => s.Price >= _min.Value);


            if (_max.HasValue)
                profilter = profilter.Where(s => s.Price <= _max.Value);


            return View(profilter.ToList());
        }
        // GET: Product
        public ActionResult IndexAdmin(decimal? _min, decimal? _max)//lọc giá
        {
            var profilter = db.Products.AsQueryable();

            if (_min.HasValue)
                profilter = profilter.Where(s => s.Price >= _min.Value);


            if (_max.HasValue)
                profilter = profilter.Where(s => s.Price <= _max.Value);


            return View(profilter.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product pro)
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");

            if (ModelState.IsValid)
            {
                if (pro.ImagePath != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(pro.ImagePath.FileName);
                    string extention = Path.GetExtension(pro.ImagePath.FileName);

                    fileName = fileName + extention;

                    pro.ImagePath.SaveAs(Path.Combine(Server.MapPath("~/images/"), fileName));
                    pro.ImagePro = "~/images/" + fileName;
                }

                db.Products.Add(pro);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            return View(pro);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var pro = db.Products.Find(id);
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product pro)
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var proEdit = db.Products.Find(pro.ProductID);
            if (ModelState.IsValid)
            {


                proEdit.NamePro = pro.NamePro;
                proEdit.DecriptionPro = pro.DecriptionPro;
                proEdit.Price = pro.Price;
                proEdit.IDCate = pro.IDCate;


                if (pro.ImagePath != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(pro.ImagePath.FileName);
                    string extention = Path.GetExtension(pro.ImagePath.FileName);

                    fileName = fileName + extention;

                    pro.ImagePath.SaveAs(Path.Combine(Server.MapPath("~/images/"), fileName));
                    proEdit.ImagePro = "~/images/" + fileName;
                }

                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }

            return View(pro);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var pro = db.Products.Find(id);
            return View(pro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Product pro, int id)
        {

            try
            {
                ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
                var proDelete = db.Products.Find(id);
                db.Products.Remove(proDelete);
                db.SaveChanges();
                ViewBag.errDelete = null;
                return RedirectToAction("IndexAdmin");
            }
            catch
            {
                ViewBag.errDelete = "Không thể xóa sản phẩm này, vì sản phẩm này đang được sử dụng!";
            }
            return View(pro);
        }
    }
}