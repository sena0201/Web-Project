using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020413.DomainModels;
using _20T1020413.BusinessLayers;
using _20T1020413.Web.Models;
using System.Globalization;

namespace _20T1020413.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 6;
        private const string EMPLOYEE_SEARCH = "SearchEmployeeCondition";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Employee
        //public ActionResult Index(int page = 1, int pageSize = 6, string searchValue = "")
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfEmloyees(page, pageSize, searchValue, out rowCount);

        //    int pageCount = rowCount / pageSize;
        //    if (rowCount % pageSize > 0)
        //        pageCount += 1;


        //    ViewBag.Page = page;
        //    ViewBag.PageCount = pageCount;
        //    ViewBag.RowCount = rowCount;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.SearchValue = searchValue;

        //    return View(data);
        //}

        public ActionResult Index()
        {

            PaginationSearchInput condition = Session[EMPLOYEE_SEARCH] as PaginationSearchInput;

            if (condition == null)
            {
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(condition);
        }

        public ActionResult Search(PaginationSearchInput condition)
        {

            int rowCount = 0;
            var data = CommonDataService.ListOfEmloyees(condition.Page, condition.PageSize, condition.SearchValue, out rowCount);
            var result = new EmployeeSearchOuput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session[EMPLOYEE_SEARCH] = condition;

            return View(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Employee()
            {
                EmployeeID = 0
            };

            ViewBag.Title = "Bổ sung nhân viên";
            return View("Edit",data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Save(Employee data, string birthday, HttpPostedFileBase uploadPhoto)
        {
            DateTime? d = Converter.DMYStringToDateTime(birthday);
            DateTime minDate = DateTime.ParseExact("2/1/1753", "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime maxDate = DateTime.ParseExact("30/12/9999", "d/M/yyyy", CultureInfo.InvariantCulture);
            if (d == null)
                ModelState.AddModelError("BirthDate", $"Ngày { birthday}  không hợp lệ. Vui lòng nhập theo định dạng dd/MM/yyyy");
            else if (d < minDate || d > maxDate)
            {
                ModelState.AddModelError("BirthDate", $"Ngày { birthday}  không hợp lệ.");
            }
            else
                data.BirthDate = d.Value;

            if (string.IsNullOrWhiteSpace(data.FirstName))
                ModelState.AddModelError("FirstName", "Tên không được để trống");

            if (string.IsNullOrWhiteSpace(data.LastName))
                ModelState.AddModelError("LastName", "Họ không được để trống");

            if (string.IsNullOrWhiteSpace(data.Photo))
                data.Photo = "";

            if (string.IsNullOrWhiteSpace(data.Notes))
                ModelState.AddModelError("Notes", "Ghi chú không được để trống");

            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError("Email", "Email không được để trống");

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/Photo");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(filePath);
                data.Photo = fileName;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
                return View("Edit", data);
            }

            if (data.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(data);
            }
            else
            {
                CommonDataService.UpdateEmployee(data);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var data = CommonDataService.GetEmployee(id);

            if (data == null) return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật nhân viên";
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (CommonDataService.GetEmployee(id) == null) return RedirectToAction("Index");

            if (Request.HttpMethod == "GET")
            {
                var data = CommonDataService.GetEmployee(id);
                return View(data);
            }
            else
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
        }
    }
}