using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020413.DomainModels;

namespace _20T1020413.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lí dữ liệu trên nhà cung cấp
    /// Sử dụng cách này dẫn đến viết lặp đi lặp lại các kiểu code giống nhau
    /// cho các đối tượng dữ liệu tương tự ...) => không đúng
    /// => dùng cách viết Generic
    /// </summary>
    public interface ISupplierDAL
    {
        /// <summary>
        /// Lấy thông tin của nhà  cung cấp theo ID
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        Supplier Get(int SupplierID);
        /// <summary>
        /// Tìm kiếm và lấy danh sách các nhà cung cấp dưỡi dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng hiển thị trên 1 trang(0 tức là không yêu cầu phân trang)</param>
        /// <param name="searchValue">Tên cần tìm kiếm (chuỗi rỗng nếu không tìm kiếm theo tên)</param>
        /// <returns></returns>
        IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "");
        /// <summary>
        /// Đếm số nhà cung cấp tìm được
        /// </summary>
        /// <param name="searchValue">Tên cần tìm kiếm (chuỗi rỗng nếu không tìm kiếm theo tên)</param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Bổ sung thêm nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID nhà cung cấp được bổ sung</returns>
        int Add(Supplier data);
        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);
        /// <summary>
        /// Xóa 1 nhà cung cấp dựa vào mà nhà cung cấp
        /// </summary>
        /// <param name="SupplierID">Mã nhà cung cấp cần xóa</param>
        /// <returns></returns>
        bool Delete(int SupplierID);
        /// <summary>
        /// Kiểm tra xem nhà cung cấp có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        bool InUsed(int SupplierID);
    }
}
