using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanHang001.Models;

namespace BanHang001.Controllers
{
    public class LoginRegisterController : Controller
    {
        MyStoreEntities1 db = new MyStoreEntities1();
        // GET: LoginRegister
        public ActionResult Index()
        {
            return View();
        }
        //xử lý login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminUser adminUser)
        {
            var checkID = db.AdminUsers.
                Where(s => s.ID.Equals(adminUser.ID)).FirstOrDefault();
            var checkPass = db.AdminUsers.
                Where(s => s.PasswordUser.Equals(adminUser.PasswordUser)).FirstOrDefault();

            if (checkID == null)
            {
                ViewBag.ErrLoginID = "Sai ID";
                return View("Index");
            }
            if (checkPass == null)
            {
                ViewBag.ErrLoginPass = "Sai mật khẩu";
                return View("Index");
            }
            if (checkID != null && checkPass != null)
            {
                Session["ID"] = adminUser.ID;
                Session["RoleUser"] = adminUser.RoleUser;
                if (Session["RoleUser"].ToString() == "admin")
                    return RedirectToAction("IndexAdmin", "Product");
                else
                    return RedirectToAction("IndexCus", "Product");
            }
            return View("Index");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "LoginRegister");
        }
        //tạo form Register
        public ActionResult Register()
        {
            return View();
        }
        //xác thực đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AdminUser adminUser)
        {
            var checkID = db.AdminUsers.Where(s => s.ID == adminUser.ID).FirstOrDefault();
            if (checkID != null)
            {
                ViewBag.ErrRegister = "ID đã tồn tại";
                return View("Register");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.AdminUsers.Add(adminUser);
                    db.SaveChanges();
                    return RedirectToAction("IndexAdmin", "LoginRegister");
                }
                return View();
            }

        }
    }
}