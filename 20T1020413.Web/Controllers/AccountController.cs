﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using _20T1020413.BusinessLayers;
using _20T1020413.DomainModels;

namespace _20T1020413.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            var cookie = Converter.CookieToUserAccount(User.Identity.Name);
            if (cookie != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userName = "", string password = "")
        {
            if (Request.HttpMethod == "GET")
            {
                return View();
            }
            ViewBag.UserName = userName;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập đủ thông tin");
                return View();
            }
            var userAccount = UserAccountService.Authorize(AccountTypes.Employee, userName, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("", "Đăng nhập thất bại");
                return View();
            }

            string cookieValue = Newtonsoft.Json.JsonConvert.SerializeObject(userAccount);
            FormsAuthentication.SetAuthCookie(cookieValue, false);
            return RedirectToAction("Index", "Home");

        }
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string userName = "", string oldPassword = "", string newPassword = "")
        {
            ViewBag.UserName = userName;
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Vui lòng nhập đủ thông tin!");
                return View();

            }
            var check = UserAccountService.ChangePassword(AccountTypes.Employee, userName, oldPassword, newPassword);
            if (check == false)
            {
                ModelState.AddModelError("", "Thông tin không chính xác!");
                return View();
            }

            if(newPassword == oldPassword)
            {
                ModelState.AddModelError("", "Mật khẩu mới không được trùng với mật khẩu cũ!");
                return View();
            }

            Response.Write("<script>alert('Đổi mật khẩu thành công!Vui lòng đăng nhập lại.')</script>");
            return View("Login");
        }

    }
}