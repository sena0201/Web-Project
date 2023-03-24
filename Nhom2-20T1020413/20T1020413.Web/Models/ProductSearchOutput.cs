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
    public class ProductSearchOuput : PaginationSearchOutput
    {
        public List<Product> Data { get; set; }
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
    }
}