using _20T1020413.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020413.Web.Models
{
    public class OrderModel : Order
    {
        public List<OrderDetail> DataDetails { get; set; }

    }
}