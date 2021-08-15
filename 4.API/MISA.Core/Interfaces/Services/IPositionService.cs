using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IPositionService
    {
        /// <summary>
        /// Thêm mới vị trí vào trong db
        /// </summary>
        /// <param name="position">Dữ liệu vị trí muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Add(Position position);

        /// <summary>
        /// Sửa 1 vị trí trong db
        /// </summary>
        /// <param name="position">Dữ liệu vị trí muốn sửa</param>
        /// <param name="positionId">Id vị trí muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Update(Position position, Guid positionId);
    }
}
