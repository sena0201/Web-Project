using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020413.DomainModels;
using _20T1020413.BusinessLayers;
using _20T1020413.Web.Models;

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
            if (d == null)
                ModelState.AddModelError("BirthDate", $"Ngày {birthday} không hợp lệ! Vui lòng nhập theo định dạng dd/MM/yyyy");
            else
                data.BirthDate = d.Value;
            try
            {
                //Kiểm soát đầu vào
                if (string.IsNullOrWhiteSpace(data.LastName))
                    ModelState.AddModelError("LastName", "Họ đệm không được để trống");

                if (string.IsNullOrWhiteSpace(data.FirstName))
                    ModelState.AddModelError("FirstName", "Tên nhân viên không được để trống");

                if (string.IsNullOrWhiteSpace(data.BirthDate.ToString()))
                    ModelState.AddModelError("BirthDate", "Ngày sinh không được để trống");

                if (string.IsNullOrWhiteSpace(data.Photo))
                    //ModelState.AddModelError("Photo", "Vui lòng tải lên hình ảnh");
                    data.Photo = "";

                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError("Email", "Email được để trống");

                if (string.IsNullOrWhiteSpace(data.Notes))
                    ModelState.AddModelError("Notes", "Ghi chú được để trống");

                //...

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";
                    return View("Edit", data);
                }

                if(uploadPhoto != null)
                {
                    string path = Server.MapPath("~/Photo");
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string filePath = System.IO.Path.Combine(path, fileName);
                    uploadPhoto.SaveAs(filePath);
                    data.Photo = fileName;
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
            catch
            {
                //Ghi lại log lỗi
                return Content("Có lỗi xảy ra. Vui lòng thử lại!!");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            if (id == 0) return RedirectToAction("Index");

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
            if (id == 0) return RedirectToAction("Index");

            if (Request.HttpMethod == "GET")
            {
                var data = CommonDataService.GetEmployee(id);
                if (data == null) return RedirectToAction("Index");
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