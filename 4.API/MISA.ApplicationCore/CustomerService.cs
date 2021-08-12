using Dapper;
using MISA.ApplicationCore.Entity;
using MISA.Entity;
using MISA.Infrastructure;
using MISA.Infrastructure.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore
{
    public class CustomerService
    {
        #region Method

        // Lấy danh sách khách hàng
        public IEnumerable<Customer> GetCustomers()
        {
            var customerContext = new CustomerContext();
            var customers = customerContext.GetCustomers();
            return customers;
        }

        // Lấy thông tin khách hàng theo Id khách hàng
        public Customer GetCustomerById(Guid customerId)
        {
            var customerContext = new CustomerContext();
            var customer = customerContext.GetCustmerById(customerId);
            return customer;
        }

        // Thêm mới khách hàng
        public ServiceResult InsertCustomer(Customer customer)
        {
            var serviceResult = new ServiceResult();
            var customerContext = new CustomerContext();
            // validate dữ liệu
            // Check trường mã bắt buộc nhập,nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi
            var customerCode = customer.CustomerCode;
            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "CustomerCode", msg = "Mã khách hàng không được phép để trống" },
                    userMsg = "Mã khách hàng không được phép để trống",
                    Code = MISAEnum.NotValid,
                };
                serviceResult.MISAErrorCode = MISAEnum.NotValid;
                serviceResult.Message = "Mã khách hàng không được phép để trống";
                serviceResult.Data = msg;
                return serviceResult;
            }

            // check trùng mã
            var row = customerContext.GetCustomerByCode(customerCode);
            if (row != null)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "CustomerCode", msg = "Mã khách hàng đã tồn tại" },
                    userMsg = "Mã khách hàng đã tồn tại",
                    Code = MISAEnum.NotValid,
                };
                serviceResult.MISAErrorCode = MISAEnum.NotValid;
                serviceResult.Message = "Mã khách hàng đã tồn tại";
                serviceResult.Data = msg;
                return serviceResult;
            }

            // thêm mới khi dữ liệu đã hợp lệ
            var rowAffect = customerContext.InsertCustomer(customer);
            serviceResult.MISAErrorCode = MISAEnum.IsValid;
            serviceResult.Message = "Thêm thành công";
            serviceResult.Data = rowAffect;
            return serviceResult;
        }

        // Sửa khách hàng theo Id khách hàng

        // Xóa khách hàng theo Id khách hàng


        #endregion
    }
}
