﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020413.DomainModels
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        public int EmployeeID { get; set; }
        /// <summary>
        /// Họ đệm
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// Ảnh
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

    }
}
