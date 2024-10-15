using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly StoreContextDB db;
        public CartController(StoreContextDB context)
        {
            db = context;
        }
        public IActionResult Index(string id)
        {
            var carts = db.Carts.Where(p=>p.UserId == id).ToList();
            var Products = db.Products.ToList();
            var Images = db.ProductImages.ToList();
            ViewBag.Products = Products;
            ViewBag.Images = Images;
            return View(carts);
        }
        [HttpPost]
        public IActionResult Save(AddCartVM model)
        {

          


            int prodid = model.ProductId;
            var cart = new Cart
                {
                    UserId = model.UserId,
                    ProductId = prodid,
                    Qty = model.Qty,
                };

                db.Carts.Add(cart);
                db.SaveChanges();
            TempData["SuccessMessage"] = "Product added to cart successfully!";
 

            return RedirectToAction("Index","Product");

           
        }


        public IActionResult Edit(int id)
        {
            Cart cart = db.Carts.SingleOrDefault(c => c.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            Product prod = db.Products.SingleOrDefault(p => p.Id == cart.ProductId);
            List<ProductImage> Images = db.ProductImages.Where(p => p.ProductId == cart.ProductId).ToList();
            ViewBag.image = Images.First();
            ViewBag.Product = prod;
            return View(cart);
        }
        [HttpPost]
        public IActionResult Update(int id,AddCartVM model)
        {
            Cart cart = db.Carts.SingleOrDefault(c => c.Id == id);
            if (cart == null) { return NotFound(); }
            cart.Qty = model.Qty;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = cart.UserId });
        }


        public IActionResult Delete(int id)
        {
            Cart cart = db.Carts.SingleOrDefault(c => c.Id == id);
            string UserId = cart.UserId;
            if (cart == null) { return NotFound(); }
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = UserId });

        }

    }
}
