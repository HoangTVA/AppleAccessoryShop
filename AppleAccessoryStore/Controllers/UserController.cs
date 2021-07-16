using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppleAccessoryStore.Controllers
{
    public class UserController : Controller
    {
        IUserRepository userRepository = null;
        public UserController() => userRepository = new UserRepository();
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TblUser user)
        {
            if (ModelState.IsValid)
            {
                TblUser User = userRepository.Login(user.UserEmail, user.UserPassword);
               
                if (User != null  )
                {
                    if (!User.RoleId.Trim().Equals("BAN")) {
                        HttpContext.Session.SetString("UserID", user.UserId.ToString());
                        HttpContext.Session.SetInt32("userId", User.UserId);
                        HttpContext.Session.SetString("userName", User.UserEmail.ToString());
                        HttpContext.Session.SetString("Role", User.RoleId.ToString());
                        return RedirectToAction("Index", "Product");
                    }
                    else {
                        ViewBag.Message = "Your account was banned from the system";
                        return View();
                    }
                    
                }
             
            }
            ViewBag.Message = "Wrong email or password";
            return View(user);
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TblUser user, string txtConfirm)
        {
            IEnumerable<TblUser> users = userRepository.GetUsers();
            int id = users.Count();
            id = id + 1;
            user.UserId = id;
            user.RoleId = "CUS";
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.UserPassword.Equals(txtConfirm))
                    {
                        userRepository.InsertUser(user);
                    }
                    else
                    {
                        ViewBag.Message = "Confirm password do not match";
                        return RedirectToAction(nameof(Register));
                    }
                }
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(user);
            }
        }
        public ActionResult BanUser(int? userID)
        {
            var user = userRepository.GetUserById(userID.Value);
            user.RoleId = "BAN";
            userRepository.UpdateUser(user);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult AdminUserList()
        {
            var userList = userRepository.GetUsers();
            return View(userList);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("userName");
            HttpContext.Session.Remove("Role");
            return RedirectToAction(nameof(Login));
        }

        public ActionResult Profile(int? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            TblUser user = userRepository.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
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

        public ActionResult Edit(int? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var user = userRepository.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int userId, TblUser user)
        {
            try
            {
                if (userId != user.UserId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    userRepository.UpdateUser(user);
                }
                return RedirectToAction(nameof(Profile), new { userId = userId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
