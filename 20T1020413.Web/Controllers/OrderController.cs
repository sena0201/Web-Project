﻿using _20T1020413.BusinessLayers;
using _20T1020413.DomainModels;
using _20T1020413.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020413.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("Order")]
    public class OrderController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";
        private const string ERROR_MESSAGE = "ErrorMessage";
        private const string SUCCESS_MESSAGE = "SuccessMessage";
        private const string ORDER_SEARCH = "SearchOrderCondition";
        private const int PAGE_SIZE = 10;

        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //TODO: Code chức năng tìm kiếm, phân trang cho đơn hàng
            OrderSearchInput condition = Session[ORDER_SEARCH] as OrderSearchInput;
            if (condition == null)
            {
                condition = new OrderSearchInput()
                {
                    EmployeeID = 0,
                    ShipperID = 0,
                    Status = 0,
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
            return View(condition);
        }

        public ActionResult Search(OrderSearchInput condition)
        {
            int rowCount = 0;
            var data = OrderService.ListOrders(condition.Page,
                condition.PageSize,
                condition.Status, condition.SearchValue,
                out rowCount);
            var result = new OrderSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                EmployeeID = condition.EmployeeID,
                ShipperID = condition.ShipperID,
                RowCount = rowCount,
                Data = data
            };
            Session[ORDER_SEARCH] = condition;
            return View(result);
        }
        /// <summary>
        /// Xem thông tin và chi tiết của đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            //TODO: Code chức năng lấy và hiển thị thông tin của đơn hàng và chi tiết của đơn hàng
            if (id <= 0)
                return RedirectToAction("Index");
            var order = OrderService.GetOrder(id);
            var data = new OrderModel()
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                CustomerName = order.CustomerName,
                CustomerAddress = order.CustomerAddress,
                CustomerEmail = order.CustomerEmail,
                CustomerContactName = order.CustomerContactName,
                OrderTime = order.OrderTime,
                EmployeeID = order.EmployeeID,
                EmployeeFullName = order.EmployeeFullName,
                AcceptTime = order.AcceptTime,
                ShipperID = order.ShipperID,
                ShipperName = order.ShipperName,
                ShipperPhone = order.ShipperPhone,
                ShippedTime = order.ShippedTime,
                FinishedTime = order.FinishedTime,
                Status = order.Status,
                DataDetails = OrderService.ListOrderDetails(id)
            };
            if (data == null)
                return RedirectToAction("Index");
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
            return View(data);
        }
        /// <summary>
        /// Giao diện Thay đổi thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("EditDetail/{orderID?}/{productID?}")]
        public ActionResult EditDetail(int orderID = 0, int productID = 0)
        {
            //TODO: Code chức năng để lấy chi tiết đơn hàng cần edit
            if (orderID <= 0 || productID <= 0)
                return RedirectToAction("Index");
            if (OrderService.GetOrder(orderID).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể sửa mặt hàng trong đơn hàng này!!!";
                return RedirectToAction($"Details/{orderID}");
            }
            var data = OrderService.GetOrderDetail(orderID, productID);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        /// <summary>
        /// Thay đổi thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDetail(OrderDetail data)
        {
            //TODO: Code chức năng để cập nhật chi tiết đơn hàng
            if (data.Quantity <= 0)
            {
                return Json(ApiResult.Fail("Số lượng không hợp lệ"), JsonRequestBehavior.AllowGet);
            }
            if (data.SalePrice <= 0)
            {
                return Json(ApiResult.Fail("Giá không hợp lệ"), JsonRequestBehavior.AllowGet);
            }


            if (OrderService.GetOrder(data.OrderID).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể xóa mặt hàng trong đơn hàng này!!!";
                return RedirectToAction($"Details/{data.OrderID}");
            }

            OrderService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            return Json(ApiResult.Sucesss());
        }

        /// <summary>
        /// Xóa 1 chi tiết trong đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("DeleteDetail/{orderID}/{productID}")]
        public ActionResult DeleteDetail(int orderID = 0, int productID = 0)
        {
            //TODO: Code chức năng xóa 1 chi tiết trong đơn hàng
            if (OrderService.GetOrder(orderID).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể xóa mặt hàng trong đơn hàng này!!!";
                return RedirectToAction($"Details/{orderID}");
            }
            if (orderID <= 0 || productID <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(orderID);
            if (data.Status == OrderStatus.INIT)
            {
                OrderService.DeleteOrderDetail(orderID, productID);
            }
            return RedirectToAction($"Details/{orderID}");
        }
        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            //TODO: Code chức năng để xóa đơn hàng (nếu được phép xóa)
            if(OrderService.GetOrder(id).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể xóa đơn hàng này!!!";
                return RedirectToAction($"Details/{id}");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (OrderService.DeleteOrder(id)){
                TempData[SUCCESS_MESSAGE] = "Xóa thành công ";
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Chấp nhận đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Accept(int id = 0)
        {
            //TODO: Code chức năng chấp nhận đơn hàng (nếu được phép)
            if (OrderService.GetOrder(id).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể duyệt đơn hàng này!!!";
                return RedirectToAction($"Details/{id}");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (OrderService.AcceptOrder(id))
            {
                TempData[SUCCESS_MESSAGE] = "Chấp nhận đơn hàng thành công";
            }
            return RedirectToAction($"Details/{id}");
        }

        public ActionResult SelectShipper(int id = 0)
        {
            if(id < 0) 
                return RedirectToAction("Index");
            ViewBag.OrderID = id;
            return View();
        }
        /// <summary>
        /// Xác nhận chuyển đơn hàng cho người giao hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Shipping(int id = 0, int shipperID = 0)
        {
            //TODO: Code chức năng chuyển đơn hàng sang trạng thái đang giao hàng (nếu được phép)
            if(shipperID == 0)
            {
                return Json(ApiResult.Fail("Vui lòng chọn shipper!"), JsonRequestBehavior.AllowGet);
            }
            if (OrderService.GetOrder(id).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể chuyển giao hàng đơn hàng này!!!";
                return RedirectToAction($"Details/{id}");
            }
            if(OrderService.ShipOrder(id, shipperID))
            {
                TempData[SUCCESS_MESSAGE] = "Chuyển giao hàng thành công";
            }
            return Json(ApiResult.Sucesss());

            //return RedirectToAction($"Details/{id}");
        }
        /// <summary>
        /// Ghi nhận hoàn tất thành công đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Finish(int id = 0)
        {
            //TODO: Code chức năng ghi nhận hoàn tất đơn hàng (nếu được phép)
            if (OrderService.GetOrder(id).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể xác nhận hoàn tất đơn hàng này!!!";
                return RedirectToAction($"Details/{id}");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (OrderService.FinishOrder(id))
            {
                TempData[SUCCESS_MESSAGE] = "Xác nhận hoàn thành đơn hàng thành công";
            }
            return RedirectToAction($"Details/{id}");
        }
        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Cancel(int id = 0)
        {
            //TODO: Code chức năng hủy đơn hàng (nếu được phép)
            if (OrderService.GetOrder(id).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể hủy đơn hàng này!!!";
                return RedirectToAction($"Details/{id}");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (OrderService.CancelOrder(id))
            {
                TempData[SUCCESS_MESSAGE] = "Hủy đơn hàng thành công";
            }
            return RedirectToAction($"Details/{id}");
        }
        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Reject(int id = 0)
        {
            //TODO: Code chức năng từ chối đơn hàng (nếu được phép)
            if (OrderService.GetOrder(id).EmployeeID != Converter.CookieToUserAccount(User.Identity.Name).UserId)
            {
                TempData[ERROR_MESSAGE] = "Không thể từ chối đơn hàng này!!!";
                return RedirectToAction($"Details/{id}");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (OrderService.RejectOrder(id))
            {
                TempData[SUCCESS_MESSAGE] = "Từ chối đơn hàng thành công";
            }
            return RedirectToAction($"Details/{id}");
        }

        /// <summary>
        /// Sử dụng 1 biến session để lưu tạm giỏ hàng (danh sách các chi tiết của đơn hàng) trong quá trình xử lý.
        /// Hàm này lấy giỏ hàng hiện đang có trong session (nếu chưa có thì tạo mới giỏ hàng rỗng)
        /// </summary>
        /// <returns></returns>
        private List<OrderDetail> GetShoppingCart()
        {
            List<OrderDetail> shoppingCart = Session[SHOPPING_CART] as List<OrderDetail>;
            if (shoppingCart == null)
            {
                shoppingCart = new List<OrderDetail>();
                Session[SHOPPING_CART] = shoppingCart;
            }
            return shoppingCart;
        }
        /// <summary>
        /// Giao diện lập đơn hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(GetShoppingCart());
        }
        /// <summary>
        /// Tìm kiếm mặt hàng để bổ sung vào giỏ hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public ActionResult SearchProducts(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(page, PAGE_SIZE, searchValue, 0, 0, out rowCount);
            ViewBag.Page = page;
            return View(data);
        }
        /// <summary>
        /// Bổ sung thêm hàng vào giỏ hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCart(OrderDetail data)
        {
            if (data == null)
            {
                TempData[ERROR_MESSAGE] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Create");
            }
            if (data.SalePrice <= 0 || data.Quantity <= 0)
            {
                TempData[ERROR_MESSAGE] = "Giá bán và số lượng không hợp lệ";
                return RedirectToAction("Create");
            }

            List<OrderDetail> shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == data.ProductID);

            if (existsProduct == null) //Nếu mặt hàng cần được bổ sung chưa có trong giỏ hàng thì bổ sung vào giỏ
            {

                shoppingCart.Add(data);
            }
            else //Trường hợp mặt hàng cần bổ sung đã có thì tăng số lượng và thay đổi đơn giá
            {
                existsProduct.Quantity += data.Quantity;
                existsProduct.SalePrice = data.SalePrice;
            }
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>
        /// <param name="id">Mã mặt hàng</param>
        /// <returns></returns>
        public ActionResult RemoveFromCart(int id = 0)
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa toàn bộ giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Khởi tạo đơn hàng (với phần thông tin chi tiết của đơn hàng là giỏ hàng đang có trong session)
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Init(int customerID = 0, int employeeID = 0)
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                TempData[ERROR_MESSAGE] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("Create");
            }

            employeeID = Converter.CookieToUserAccount(User.Identity.Name).UserId;

            if (customerID == 0)
            {
                TempData[ERROR_MESSAGE] = "Vui lòng chọn khách hàng";
                return RedirectToAction("Create");
            }

            int orderID = OrderService.InitOrder(customerID, employeeID, DateTime.Now, shoppingCart);

            Session.Remove(SHOPPING_CART); //Xóa giỏ hàng 

            return RedirectToAction($"Details/{orderID}");
        }
    }
}