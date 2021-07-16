using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository; 
namespace AppleAccessoryStore.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository orderRepository = null;
        IUserRepository userRepository = null;
        IOrderDetailRepository orderDetailRepository = null;
        public OrderController()
        {
            orderRepository = new OrderRepository();
            userRepository = new UserRepository();
            orderDetailRepository = new OrderDetailRepository();
        }
        // GET: OrderController
        public ActionResult Index()
        {
            return View();
        }
            public ActionResult OrderList()
        {
            if (HttpContext.Session.GetString("Role").Trim() == "AD")
            {
                var orderList = orderRepository.GetOrders();
                return View(orderList);
            }
            else
            {
                int userID = HttpContext.Session.GetInt32("userId").Value;
                var orderList = orderRepository.GetOrderByuID(userID);
                return View(orderList);
            }

            
            
        }
       

        // GET: OrderController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
          
            var order = orderRepository.GetOrderById(id.Value);
            var user = userRepository.GetUserById(order.UserId.Value);
            var orderDetail = orderDetailRepository.GetOrderDetailByOrderID(id.Value);
            
            if (order == null)
            {
                return NotFound();

            }
            ViewBag.user = user;
            ViewBag.order = order;
            return View(orderDetail);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
