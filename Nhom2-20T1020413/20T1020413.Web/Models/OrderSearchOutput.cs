using _20T1020413.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020413.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderSearchOutput : PaginationSearchOutput
    {
        public List<Order> Data { get; set; }
        public int EmployeeID { get; set; } = 0;
        public int ShipperID { get; set; } = 0;
        public int Status { get; set; } = 0;
    }
}