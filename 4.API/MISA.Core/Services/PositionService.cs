using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class PositionService : IPositionService
    {
        IPositionRepository _positionRepository;
        ServiceResult _serviceResult;
        public PositionService(IPositionRepository positionRepository)
        {
            _serviceResult = new ServiceResult();
            _positionRepository = positionRepository;
        }

        /// <summary>
        /// Thêm mới vị trí vào trong db
        /// </summary>
        /// <param name="position">Dữ liệu vị trí muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Add(Position position)
        {
            // Xử lý nghiệp vụ,Validate dữ liệu
            // Check trường mã vị trí bắt buộc nhập:
            var positionCode = position.PositionCode;
            if (string.IsNullOrEmpty(positionCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg__Null_PositionCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_PositionCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // thao tác với db
            _serviceResult.Data = _positionRepository.Add(position);
            _serviceResult.MISACode = MISAEnum.EnumServiceResult.Created;
            return _serviceResult;
        }

        /// <summary>
        /// Sửa 1 vị trí trong db
        /// </summary>
        /// <param name="position">Dữ liệu vị trí muốn sửa</param>
        /// <param name="positionId">Id vị trí muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Update(Position position, Guid positionId)
        {
            // Xử lý nghiệp vụ, Validate dữ liệu
            // Check trường mã vị trí bắt buộc nhập:
            var positionCode = position.PositionCode;
            if (string.IsNullOrEmpty(positionCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg__Null_PositionCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_PositionCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // thao tác với db
            _serviceResult.Data = _positionRepository.Update(position, positionId);
            _serviceResult.MISACode = MISAEnum.EnumServiceResult.Success;
            return _serviceResult;
        }
    }
}
