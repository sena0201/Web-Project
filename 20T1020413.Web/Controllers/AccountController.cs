using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using _20T1020413.BusinessLayers;
using _20T1020413.DomainModels;

namespace _20T1020411.Web.Controllers
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string userName = "" , string oldPassword = "", string newPassword = "")
        {
            if (Request.HttpMethod == "GET")
            {
                return View();
            }
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Vui lòng nhập đủ thông tin");
                return View();
            }
            var userAccount = UserAccountService.Authorize(AccountTypes.Employee, userName, oldPassword);
            if (userAccount == null)
            {
                ModelState.AddModelError("", "Thông tin không chính xác");
                return View();
            }

            UserAccountService.ChangePassword(AccountTypes.Employee, userName, oldPassword, newPassword);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePassword(string userName = "", string oldPassword = "", string newPassword = "")
        {
            try
            {
                //Kiểm soát đầu vào
                if (string.IsNullOrWhiteSpace(userName))
                    ModelState.AddModelError("", "Tên đăng nhập không được để trống");

                if (string.IsNullOrWhiteSpace(oldPassword))
                    ModelState.AddModelError("", "Vui lòng nhập mật khẩu cũ");

                if (string.IsNullOrWhiteSpace(newPassword))
                    ModelState.AddModelError("", "Vui lòng nhập mật khẩu mới");
                //...

                var userAccount = UserAccountService.Authorize(AccountTypes.Employee, userName, oldPassword);
                if (userAccount == null)
                {
                    ModelState.AddModelError("", "Thông tin không chính xác");
                    return View();
                }
                else
                {
                    UserAccountService.ChangePassword(AccountTypes.Employee, userName, oldPassword, newPassword);
                }
                return RedirectToAction("Index","Home");
            }
            catch
            {
                //Ghi lại log lỗi
                return Content("Có lỗi xảy ra. Vui lòng thử lại!!");
            }
        }
    }
}