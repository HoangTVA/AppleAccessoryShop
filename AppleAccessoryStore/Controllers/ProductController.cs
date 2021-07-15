using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject;
using System.Web;
using Newtonsoft.Json;

namespace AppleAccessoryStore.Controllers
{
   
    public class ProductController : Controller
    {
        IProductRepository productRepository = null;
        IOrderRepository orderRepository = null;
        IOrderDetailRepository detailRepository = null;
        public ProductController()
        {
            productRepository = new ProductRepository();
            orderRepository = new OrderRepository();
            detailRepository = new OrderDetailRepository();
        }
        // GET: ProductController
        public ActionResult Index()
        {
            var productList = productRepository.GetProducts();
            if (productList == null)
            {
                ViewBag.Message = "No product in store";
            }
            return View(productList);
            
        }

        public IActionResult addCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");//get key cart
            if (cart == null)
            {
                var product = productRepository.GetProductById(id);
                List<Cart> listCart = new List<Cart>()
               {
                   new Cart
                   {
                       product = product,
                       quantity = 1
                   }
               };
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));

            }
            else
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].product.ProductId == id)
                    {
                        dataCart[i].quantity++;
                        check = false;
                    }
                }
                if (check)
                {
                    dataCart.Add(new Cart
                    {
                        product = productRepository.GetProductById(id),
                        quantity = 1
                    });
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                // var cart2 = HttpContext.Session.GetString("cart");//get key cart
                //  return Json(cart2);
            }

            return RedirectToAction(nameof(Index));

        }

        public IActionResult ListCart()
        {
            var cart = HttpContext.Session.GetString("cart");//get key cart
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if (dataCart.Count > 0)
                {
                    ViewBag.carts = dataCart;
                    return View();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult deleteCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].product.ProductId == id)
                    {
                        dataCart.RemoveAt(i);
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                return RedirectToAction(nameof(ListCart));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Buy(string Address)
        {
            var cart = HttpContext.Session.GetString("cart");
            var userId = HttpContext.Session.GetInt32("userId");
            decimal? totalPrice = 0;
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (Address == null)
            {
                return RedirectToAction(nameof(ListCart));
            }
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    totalPrice = totalPrice + (dataCart[i].product.ProductPrice * dataCart[i].quantity);
                }
                int orderCount = orderRepository.GetOrders().Count() + 1;
                TblOrder order = new TblOrder
                {
                    OrderId = orderCount,
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Total = totalPrice, 
                    Address = Address
                };
                orderRepository.AddOrder(order);
                //List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    TblOrderDetail details = new TblOrderDetail
                    {
                        OrderId = orderCount,
                        ProductId = dataCart[i].product.ProductId,
                        UnitPrice = (dataCart[i].product.ProductPrice * dataCart[i].quantity),
                        Quantity = dataCart[i].quantity
                    };
                    detailRepository.addDetail(details);
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TblProduct product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productRepository.InsertProduct(product);
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        /*[HttpPost("FileUpload")]
        public async Task<IActionResult> Create(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = "~/image/";
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { files.Count, size, filePaths });
        }
        */
        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
