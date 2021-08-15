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
    public class DepartmentService : IDepartmentService
    {
        ServiceResult _serviceResult;
        IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _serviceResult = new ServiceResult();
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Thêm mới phòng ban vào trong db
        /// </summary>
        /// <param name="department">Dữ liệu phòng ban muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Add(Department department)
        {
            // xử lý nghiệp vụ,Validate dữ liệu
            // Check trường mã phòng ban bắt buộc nhập:
            var departmentCode = department.DepartmentCode;
            if (string.IsNullOrEmpty(departmentCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg__Null_DepartmentCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_DepartmentCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }
            // thao tác với db
            _serviceResult.Data = _departmentRepository.Add(department);
            _serviceResult.MISACode = MISAEnum.EnumServiceResult.Created;
            return _serviceResult;
        }

        /// <summary>
        /// Sửa 1 phòng ban trong db
        /// </summary>
        /// <param name="department">Dữ liệu phòng ban muốn sửa</param>
        /// <param name="departmentId">Id phòng ban muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Update(Department department, Guid departmentId)
        {
            // Xử lý nghiệp vụ,Validate dữ liệu
            // Check trường mã nhân viên bắt buộc nhập:
            var departmentCode = department.DepartmentCode;
            if (string.IsNullOrEmpty(departmentCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg__Null_DepartmentCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_DepartmentCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // thao tác với db
            _serviceResult.Data = _departmentRepository.Update(department, departmentId);
            _serviceResult.MISACode = MISAEnum.EnumServiceResult.Success;
            return _serviceResult;
        }
    }
}
