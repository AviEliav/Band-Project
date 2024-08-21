using BandProject.Data;
using BandProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BandProject.Controllers
{
    public class StoreController : Controller
    {
        private readonly BandProjectContext _context;

        private static List<Product> prod = new List<Product>();

        //private static List<CartItem> cart = new List<CartItem>();

        public StoreController(BandProjectContext context)
        {
            _context = context;
            prod = _context.Products.ToList();
        }

        public IActionResult Index()
        {
            double total = 0;

            if (HttpContext.Session.GetString("cart") == null)
            {
                var cart = new List<CartItem>();
                var json = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("cart", json);
            }
            var cartObject = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));

            foreach (var cartItem in cartObject)
            {
                total += cartItem.Price * cartItem.Quantity;
            }

            ViewBag.Total = total;
            ViewBag.Product = prod;
            ViewBag.Cart = cartObject;

            return View(new Purchase());
        }

        public IActionResult IndexWithPurchase(Purchase purchase)
        {
            double total = 0;

            if (HttpContext.Session.GetString("cart") == null)
            {
                var cart = new List<CartItem>();
                var json = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("cart", json);
            }
            var cartObject = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));

            foreach (var cartItem in cartObject)
            {
                total += cartItem.Price * cartItem.Quantity;
            }

            ViewBag.Total = total;
            ViewBag.Product = prod;
            ViewBag.Cart = cartObject;

            return View("Index",purchase);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var prod = _context.Products.First(x => x.Id == id);

            CartItem cartItem = new CartItem();

            cartItem.Id = prod.Id;
            cartItem.ItemName = prod.ProductName;
            cartItem.ItemType = prod.ProudctType;
            cartItem.Image = prod.Image;
            cartItem.Price = prod.Price;

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));

            if (cart.Any(y => y.Id == id))
            {
                var item = cart.FirstOrDefault(y => y.Id == id);
                item.Quantity++;
            }
            else
            {
                cartItem.Quantity = 1;
                cart.Add(cartItem);
            }

            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", json);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));
            var item = cart.First(x => x.Id == id);
            cart.Remove(item);

            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", json);

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public IActionResult Purchase(string fullName)
        //{
        //    string Pattern = @"^\w{2,}(\s+\w{2,})+$";

        //    double total = 0;

        //    var cartObject = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));

        //    foreach (var cartItem in cartObject)
        //    {
        //        total += cartItem.Price * cartItem.Quantity;
        //    }

        //    if (Regex.IsMatch(fullName, Pattern) && total>0)
        //    {
        //        Purchase purchase = new Purchase { FullName = fullName, PurchasePrice = total, PurchaseDate = DateTime.Now };
        //        _context.Purchases.Add(purchase);
        //        _context.SaveChanges();

        //        HttpContext.Session.SetString("cart", null);
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Purchase(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                double total = 0;
                var cartObject = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));

                foreach (var cartItem in cartObject)
                {
                    total += cartItem.Price * cartItem.Quantity;
                }

                purchase.PurchasePrice = total;
                purchase.PurchaseDate = DateTime.Now;
                _context.Purchases.Add(purchase);
                _context.SaveChanges();
                var cart = new List<CartItem>();
                var json = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("cart", json);
                return RedirectToAction("Index");
            }
            return RedirectToAction("IndexWithPurchase", purchase);
        }


    }
}
