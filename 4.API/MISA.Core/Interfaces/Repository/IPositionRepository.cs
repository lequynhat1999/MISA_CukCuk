using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IPositionRepository
    {
        /// <summary>
        /// Thêm mới 1 vị trí vào trong db
        /// </summary>
        /// <param name="position">dữ liệu vị trí thêm mới</param>
        /// <returns>Số bản ghi được thêm vào db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        int Add(Position position);

        /// <summary>
        /// Sửa thông tin 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí muốn sửa</param>
        /// <param name="position">dữ liệu vị trí muốn sửa</param>
        /// <returns>Số bản ghi được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        int Update(Position position, Guid positionId);

        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về vị trí trong db
        /// </summary>
        /// <returns>Danh sách vị trí</returns>
        /// CreateBy: LQNhat(9/8/2021)
        IEnumerable<Position> Get();

        /// <summary>
        /// lấy ra dữ liệu về 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí</param>
        /// <returns>Vị trí tìm theo Id</returns>
        /// CreateBy: LQNhat(9/8/2021)
        Position GetById(Guid positionId);

        /// <summary>
        /// Xóa 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">Id vị trí muốn xóa</param>
        /// <returns>Số bản ghi vừa xóa trong db</returns>
        /// CreateBy: LQNhat(09/08/2021)
        int Delete(Guid positionId);
    }
}
