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
using AppleAccessoryStore.Models;
using System.Dynamic;

namespace AppleAccessoryStore.Controllers
{
   
    public class ProductController : Controller
    {
        private Apple_Accessory_StoreContext db = new Apple_Accessory_StoreContext();
        IProductRepository productRepository = null;
        IOrderRepository orderRepository = null;
        IOrderDetailRepository detailRepository = null;
        ICommentRepository commentRepository = null;
        public ProductController()
        {
            productRepository = new ProductRepository();
            orderRepository = new OrderRepository();
            detailRepository = new OrderDetailRepository();
            commentRepository = new CommentRepository();
        }
        // GET: ProductController
        public ActionResult Index(int pg=1)
        {
            var productList = productRepository.GetProducts();
            const int pageSize = 3;
            if (pg < 1)
                pg = 1;
            int rescCount = productList.Count();
            var pager = new Pager(rescCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = productList.Skip(recSkip).Take(pager.PageSize);
            if (productList == null)
            {
                ViewBag.Message = "No product in store";
            }
            ViewBag.Pager = pager;
          
            return View(data);
            
        }

        public ActionResult AddComment(string comment, int productID)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            int comments = commentRepository.getComments().Count();
            int id = comments + 1;
            TblComment newComment = new TblComment
            {
                CommentId = id,
                UserId = userId,
                Description = comment,
                ProductId = productID
            };
            try
            {
                if (ModelState.IsValid)
                {
                    commentRepository.AddComment(newComment);
                    return RedirectToAction(nameof(Details), new { id = productID });
                }
                return RedirectToAction(nameof(Details), new { id = productID });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Index));
            }
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
                HttpContext.Session.SetString("order", JsonConvert.SerializeObject(order));
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
                return RedirectToAction(nameof(OrderCompletion));
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult OrderCompletion()
        {
            var cart = HttpContext.Session.GetString("cart");//get key cart
            var order = HttpContext.Session.GetString("order");
            if (cart != null && order !=null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                TblOrder orderData = JsonConvert.DeserializeObject<TblOrder>(order);
                ViewBag.order = orderData;
                ViewBag.cart = dataCart;
                HttpContext.Session.Remove("cart");
                HttpContext.Session.Remove("order");
                return View();
            }
            return View();
        }
        // GET: ProductController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dynamic myModel = new ExpandoObject();
            myModel.product = productRepository.GetProductById(id.Value);
            myModel.comments = commentRepository.GetCommentsByProduct(id.Value);
            if(myModel == null)
            {
                return NotFound();
            }
            return View(myModel);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblProduct product, List<IFormFile> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productRepository.InsertProduct(product);
                }
                var size = files.Sum(f => f.Length);
                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/image/"+formFile.FileName);
                        filePaths.Add(filePath);
                        using (var stream = new FileStream(filePath,FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }

                // Process uploaded files
                // Don't rely on or trust the FileName property without validation.

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = productRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TblProduct product, List<IFormFile> files)
        {
            try
            {
                if (id != product.ProductId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    productRepository.UpdateProduct(product);
                }
                var size = files.Sum(f => f.Length);
                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/" + formFile.FileName);
                        filePaths.Add(filePath);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public ActionResult Search(string searchString, int pg = 1)
        {
            if (searchString == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var productList = productRepository.SearchProduct(searchString);
            const int pageSize = 3;
            if (pg < 1)
                pg = 1;
            int rescCount = productList.Count();
            var pager = new Pager(rescCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = productList.Skip(recSkip).Take(pager.PageSize);
            if (productList == null)
            {
                ViewBag.Message = "No product can be found";
            }
            ViewBag.Pager = pager;
            ViewBag.SearchString = searchString;
            return View(data);
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

        //public ActionResult Search(string searchString)
        //{
        //    if (searchString == null)
        //    {
        //        return NotFound();
        //    }
        //    var productList = productRepository.SearchProduct(searchString);

        //    if (productList == null)
        //    {
        //        ViewBag.Message = "No product in store";
        //    }
        //    return View(productList);
        //}

        /*public ActionResult Search(string id)
        {
            string searchString = id;
            var movies = from m in db.TblProducts
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.ProductName.Contains(searchString));
            }

            return View(movies);
        }*/
    }
}
