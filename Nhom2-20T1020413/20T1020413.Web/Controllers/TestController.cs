﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020413.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult Input()
        {
            Person p = new Person()
            {
                BirthDate = new DateTime(1990,11,28)
            };
            return View(p);
        }

        public ActionResult Input(Person p)
        {
            var data = new
            {
                Name = p.Name,
                BirthDate = string.Format("{0:dd/MM/yyy}", p.BirthDate),
                Salary = p.Salary
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public string TestDate(DateTime value)
        {
            DateTime d = value;
            return string.Format("{0:dd/MM/yyyy}", d);
        }

    }
    public class Person
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; } 
        public float Salary { get; set; }
    }
}