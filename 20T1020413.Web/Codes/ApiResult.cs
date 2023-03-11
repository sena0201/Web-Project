using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020413.Web
{
    public class ApiResult
    {
        /// <summary>
        /// 0: Lỗi, 1: Thành công
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Dữ liệu (nếu có)
        /// </summary>
        public object Data { get; set; } = null;

        public static ApiResult Fail(string msg)
        {
            return new ApiResult() { Code = 0, Message = msg };
        }

        public static ApiResult Sucesss(object data = null)
        {
            return new ApiResult() { Code = 1, Message = "", Data = data };
        }
    }
}