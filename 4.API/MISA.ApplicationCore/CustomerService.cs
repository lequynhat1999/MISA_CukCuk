using MISA.Infrastructure;
using MISA.Infrastructure.Model;
using System;
using System.Collections.Generic;
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

        // Sửa khách hàng theo Id khách hàng

        // Xóa khách hàng theo Id khách hàng

        #endregion
    }
}
