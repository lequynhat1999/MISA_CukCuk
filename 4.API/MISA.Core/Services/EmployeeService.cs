using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _serviceResult = new ServiceResult();
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Thêm mới nhân viên vào trong db
        /// </summary>
        /// <param name="employee">Dữ liệu nhân viên muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Add(Employee employee)
        {
            // xử lý nghiệp vụ, validate data
            // Check trường mã nhân viên bắt buộc nhập:
            var employeeCode = employee.EmployeeCode;
            if (string.IsNullOrEmpty(employeeCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg__Null_EmployeeCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_EmployeeCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // Check họ và tên nhân viên bắt buộc nhập
            var employeeFullName = employee.FullName;
            if (string.IsNullOrEmpty(employeeFullName))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_FullName,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_FullName,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // Check Email bắt buộc nhập
            var employeeEmail = employee.Email;
            if (string.IsNullOrEmpty(employeeEmail))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_Email,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_Email,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // Check SĐT bắt buộc nhập
            var employeePhoneNumber = employee.PhoneNumber;
            if (string.IsNullOrEmpty(employeePhoneNumber))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_PhoneNumber,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_PhoneNumber,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // check CMTND bắt buộc nhập
            var employeeIdentify = employee.IdentityNumber;
            if (string.IsNullOrEmpty(employeeIdentify))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_IdentityNumber,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_IdentityNumber,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // check email đúng định dạng
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var isMatch = Regex.IsMatch(employee.Email, emailFormat, RegexOptions.IgnoreCase);
            if (isMatch == false)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_FormatEmail,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_FormatEmail,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // check trùng mã
            var row = _employeeRepository.GetByCode(employeeCode, Guid.Empty);

            if (row != null)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Check_EmployeeCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Check_EmployeeCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }
            // thao tác với db
            _serviceResult.Data = _employeeRepository.Add(employee);
            _serviceResult.MISACode = MISAEnum.EnumServiceResult.Created;
            return _serviceResult;
        }

        /// <summary>
        /// Sửa 1 nhân viên trong db
        /// </summary>
        /// <param name="employee">Dữ liệu nhân viên muốn sửa</param>
        /// <param name="employeeId">Id nhân viên muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Update(Employee employee, Guid employeeUpdateId)
        {
            // xử lý nghiệp vụ, validate data
            // Check trường mã nhân viên bắt buộc nhập:
            var employeeCode = employee.EmployeeCode;
            var employeeId = employee.EmployeeId;
            if (string.IsNullOrEmpty(employeeCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg__Null_EmployeeCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_EmployeeCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // Check họ và tên nhân viên bắt buộc nhập
            var employeeFullName = employee.FullName;
            if (string.IsNullOrEmpty(employeeFullName))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_FullName,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_FullName,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // Check Email bắt buộc nhập
            var employeeEmail = employee.Email;
            if (string.IsNullOrEmpty(employeeEmail))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_Email,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_Email,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // Check SĐT bắt buộc nhập
            var employeePhoneNumber = employee.PhoneNumber;
            if (string.IsNullOrEmpty(employeePhoneNumber))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_PhoneNumber,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_PhoneNumber,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // check CMTND bắt buộc nhập
            var employeeIdentify = employee.IdentityNumber;
            if (string.IsNullOrEmpty(employeeIdentify))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Null_IdentityNumber,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Null_IdentityNumber,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // check email đúng định dạng
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var isMatch = Regex.IsMatch(employee.Email, emailFormat, RegexOptions.IgnoreCase);
            if (isMatch == false)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_FormatEmail,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_FormatEmail,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // check trùng mã
            var row = _employeeRepository.GetByCode(employeeCode, employeeId);

            if (row != null)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnEmployee.Dev_ErrorMsg_Check_EmployeeCode,
                    userMsg = Resources.ResourceVnEmployee.User_ErrorMsg_Check_EmployeeCode,
                };
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Data = msg;
                return _serviceResult;
            }

            // thao tác với db
            _serviceResult.Data = _employeeRepository.Update(employee, employeeUpdateId);
            _serviceResult.MISACode = MISAEnum.EnumServiceResult.Success;
            return _serviceResult;
        }


    }
}
