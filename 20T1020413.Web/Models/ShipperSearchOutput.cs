using _20T1020413.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020413.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm người giao hàng dưới dạng phân trang
    /// </summary>
    public class ShipperSearchOutput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách người giao hàng
        /// </summary>
        public List<Shipper> Data { get; set; }
    }
}