using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về phòng ban trong db
        /// </summary>
        /// <returns>Danh sách phòng ban</returns>
        /// CreateBy: LQNhat(9/8/2021)
        IEnumerable<Department> Get();

        /// <summary>
        /// lấy ra dữ liệu về 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban</param>
        /// <returns>Phòng ban tìm kiếm theo Id</returns>
        /// CreateBy: LQNhat(9/8/2021)
        Department GetById(Guid departmentId);

        /// <summary>
        /// Thêm mới 1 phòng ban vào trong db
        /// </summary>
        /// <param name="department">dữ liệu phòng ban thêm mới</param>
        /// <returns>Số bản ghi được thêm vào db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        int Add(Department department);

        /// <summary>
        /// Sửa thông tin 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban muốn sửa</param>
        /// <param name="department">dữ liệu phòng ban muốn sửa</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        int Update(Department department, Guid departmentId);

        /// <summary>
        /// Xóa 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">Id phòng ban muốn xóa</param>
        /// <returns>Số bản ghi đã xóa trong db</returns>
        /// CreateBy: LQNhat(09/08/2021)
        int Delete(Guid departmentId);
    }
}
