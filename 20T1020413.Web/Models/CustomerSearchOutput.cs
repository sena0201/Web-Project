using _20T1020413.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020413.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm khách hàng dưới dạng phân trang
    /// </summary>
    public class CustomerSearchOutput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách khách hàng
        /// </summary>
        public List<Customer> Data { get; set; }
    }
}