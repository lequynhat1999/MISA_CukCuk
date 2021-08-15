using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Thêm mới phòng ban vào trong db
        /// </summary>
        /// <param name="department">Dữ liệu phòng ban muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Add(Department department);

        /// <summary>
        /// Sửa 1 phòng ban trong db
        /// </summary>
        /// <param name="department">Dữ liệu phòng ban muốn sửa</param>
        /// <param name="departmentId">Id phòng ban muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Update(Department department, Guid departmentId);
    }
}
