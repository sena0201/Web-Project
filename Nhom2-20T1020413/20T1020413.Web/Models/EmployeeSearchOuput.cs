using _20T1020413.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020413.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm nhân viên dưới dạng phân trang
    /// </summary>
    public class EmployeeSearchOuput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        public List<Employee> Data { get; set; }
    }
}