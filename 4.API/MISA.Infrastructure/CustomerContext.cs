using Dapper;
using MISA.CukCuk.Api.Model;
using MISA.Infrastructure.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure
{
    public class CustomerContext
    {
        #region Method
        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreateBy: LQNhat(11/08/2021)
        public IEnumerable<Customer> GetCustomers()
        {
            // kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer";
            var customers = dbConnection.Query<Customer>(sqlQuery);

            // trả về client
            return customers;
        }

        // Lấy thông tin khách hàng theo id khách hàng
        public Customer GetCustmerById(Guid customerId)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer WHERE CustomerId = @customerId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerId", customerId);
            var customer = dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);

            // 4. trả về client
            return customer;
        }

        // Thêm mới khách hàng

        // Sửa thông tin khách hàng

        // Xóa thông tin khách hàng theo id khách hàng

        #endregion
    }
}
