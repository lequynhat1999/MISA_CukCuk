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
    public class CustomerService : ICustomerService
    {
        ICustomerRepository _customerRepository;
        ServiceResult _serviceReusult;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _serviceReusult = new ServiceResult();
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Thêm mới khách hàng vào trong db
        /// </summary>
        /// <param name="customer">Dữ liệu khách hàng muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Add(Customer customer)
        {
            // Xử lý nghiệp vụ
            // Check trường mã khách hàng bắt buộc nhập:
            var customerCode = customer.CustomerCode;
            var customerId = customer.CustomerId;
            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg__Null_CustomerCode,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_CustomerCode,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // Check họ và tên khách hàng bắt buộc nhập
            var customerFullName = customer.FullName;
            if (string.IsNullOrEmpty(customerFullName))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Null_FullName,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_FullName,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // Check Email bắt buộc nhập
            var customerEmail = customer.Email;
            if (string.IsNullOrEmpty(customerEmail))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Null_Email,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_Email,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // Check SĐT bắt buộc nhập
            var customerPhoneNumber = customer.PhoneNumber;
            if (string.IsNullOrEmpty(customerPhoneNumber))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Null_PhoneNumber,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_PhoneNumber,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // check email đúng định dạng
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var isMatch = Regex.IsMatch(customer.Email, emailFormat, RegexOptions.IgnoreCase);
            if (isMatch == false)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_FormatEmail,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_FormatEmail,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // check trùng mã
            var row = _customerRepository.GetByCode(customerId, customerCode);
            if (row != null)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Check_CustomerCode,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Check_CustomerCode,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }
            // Thao tác với Db
            _serviceReusult.Data = _customerRepository.Add(customer);
            _serviceReusult.MISACode = MISAEnum.EnumServiceResult.Created;
            return _serviceReusult;
        }

        /// <summary>
        /// Sửa 1 khách hàng trong db
        /// </summary>
        /// <param name="customer">Dữ liệu khách hàng muốn sửa</param>
        /// <param name="customerId">Id khách hàng muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public ServiceResult Update(Customer customer, Guid customerUpdateId)
        {
            // xử lý nghiệp vụ
            // Xử lý nghiệp vụ
            // Check trường mã khách hàng bắt buộc nhập:
            var customerCode = customer.CustomerCode;
            var customerId = customer.CustomerId;
            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg__Null_CustomerCode,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_CustomerCode,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // Check họ và tên khách hàng bắt buộc nhập
            var customerFullName = customer.FullName;
            if (string.IsNullOrEmpty(customerFullName))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Null_FullName,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_FullName,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // Check Email bắt buộc nhập
            var customerEmail = customer.Email;
            if (string.IsNullOrEmpty(customerEmail))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Null_Email,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_Email,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // Check SĐT bắt buộc nhập
            var customerPhoneNumber = customer.PhoneNumber;
            if (string.IsNullOrEmpty(customerPhoneNumber))
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Null_PhoneNumber,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Null_PhoneNumber,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // check email đúng định dạng
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var isMatch = Regex.IsMatch(customer.Email, emailFormat, RegexOptions.IgnoreCase);
            if (isMatch == false)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_FormatEmail,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_FormatEmail,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // check trùng mã
            var row = _customerRepository.GetByCode(customerId, customerCode);
            if (row != null)
            {
                var msg = new
                {
                    devMsg = Resources.ResourceVnCustomer.Dev_ErrorMsg_Check_CustomerCode,
                    userMsg = Resources.ResourceVnCustomer.User_ErrorMsg_Check_CustomerCode,
                };
                _serviceReusult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceReusult.Data = msg;
                return _serviceReusult;
            }

            // thao tác với db
            _serviceReusult.Data = _customerRepository.Update(customer, customerUpdateId);
            _serviceReusult.MISACode = MISAEnum.EnumServiceResult.Success;
            return _serviceReusult;
        }

    }
}
