using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Tự sinh mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới dạng string</returns>
        /// CreateBy: LQNHAT(13/08/2021)
        string NewCode();

        /// <summary>
        /// Lọc danh sách nhân viên theo các tiêu chí và phân trang
        /// </summary>
        /// <param name="pageIndex">Index của trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi hiển thị trên 1 trang</param>
        /// <param name="positionId">Id của vị trí cần tìm kiếm</param>
        /// <param name="departmentId">Id của phòng ban cần tìm kiếm</param>
        /// <param name="keysearch">Mã nhân viên, Họ và tên, SĐT cần tìm kiếm</param>
        /// <returns>Danh sách các bản ghi theo điều kiện lọc</returns>
        /// CreateBy: LQNHAT(14/08/2021)
        IEnumerable<Employee> GetByPaging(int pageIndex, int pageSize, string positionId, string departmentId, string keysearch);

        /// <summary>
        /// Lấy ra 1 nhân viên theo mã nhân viên
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>1 nhân viên được tìm kiếm theo mã</returns>
        /// CreateBy:LQNhat(10/08/2021)
        Employee GetByCode(string employeeCode, Guid? employeeId);
    }
}
