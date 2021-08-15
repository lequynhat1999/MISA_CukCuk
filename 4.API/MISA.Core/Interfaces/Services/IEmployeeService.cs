using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Thêm mới nhân viên vào trong db
        /// </summary>
        /// <param name="employee">Dữ liệu nhân viên muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Add(Employee employee);

        /// <summary>
        /// Sửa 1 nhân viên trong db
        /// </summary>
        /// <param name="employee">Dữ liệu nhân viên muốn sửa</param>
        /// <param name="employeeId">Id nhân viên muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Update(Employee employee, Guid employeeId);
    }
}
