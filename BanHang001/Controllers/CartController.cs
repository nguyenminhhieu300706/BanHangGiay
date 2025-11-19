using BanHang001.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanHang001.Models;

namespace BanHang001.Controllers
{
    public class CartController : Controller
    {
        //khoi tao
        MyStoreEntities1 db = new MyStoreEntities1();
        //tạo Session giỏ hàng
        public List<CartItem > GetCartItems()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }
        //viet ham them san pham vao gio hang - cart
        public ActionResult Add(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return Content("Sản phẩm này đã bị lỗi");
            }
            //show giỏ hàng để add sản phẩm vào giỏ hàng
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(s => s.idPro == id);
            //nếu sp này đã có trong giỏ hàng thì tăng soos lượng
            if (item != null) item.quantityPro++;
            //ngược lại thì thêm dòng sp vào giỏ hàng
            else cart.Add(new CartItem
            {
                idPro = product.ProductID,
                namePro = product.NamePro,
                pricePro = (decimal)product.Price,
                quantityPro = 1,
            });
            return RedirectToAction("Index");
        }
        // GET: tạo giao diện cho giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCartItems();
            ViewBag.Total = cart.Sum(s => s.total);
            ViewBag.Count = cart.Count();
            return View(cart);
        }
        //xóa dòng hàng
        public ActionResult Remove(int id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(s => s.idPro == id);
            if (item != null) cart.Remove(item);
            return RedirectToAction("Index");
        }
        //thanh toán đơn hàng
        public ActionResult ThanhToan(int idCus, string DiaChiNhanHang)
        {
            var cart = GetCartItems();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống";
                return RedirectToAction("Index");
            }
            else
            {
                var order = new OrderPro
                {
                    IDCus = idCus,
                    DateOrder = DateTime.Now,
                    AddressDeliverry = DiaChiNhanHang,
                    TotalAmount = cart.Sum(s => s.total)
                };
                db.OrderProes.Add(order);
                db.SaveChanges();
                //lưu từng dòng chi tiết hóa đơn
                foreach (var item in cart)
                {
                    var orderdetail = new OrderDetail
                    {
                        IDOrder = order.ID,
                        IDProduct = item.idPro,
                        UnitPrice = (double?)item.pricePro,
                        Quantity = item.quantityPro,
                    };
                }
                db.SaveChanges();
                //sau khi lưu du liệu thì giỏ hàng rỗng
                Session["Cart"] = null;
                TempData["Success"] = "Thanh toán thành công";
                return RedirectToAction("IndexCus", "Product");
            }
        }
    }
}