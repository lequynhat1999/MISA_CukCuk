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

        /// <summary>
        /// Lấy ra 1 khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id khách hàng cần tìm</param>
        /// <returns>1 khách hàng</returns>
        /// CreateBy: LQNhat(11/08/2021)
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

        /// <summary>
        /// Thêm một bản ghi vào trong db
        /// </summary>
        /// <param name="customer">Dữ liệu khách hàng muốn thêm</param>
        /// <returns>Số bản ghi đã được thêm vào trong db</returns>
        public int InsertCustomer(Customer customer)
        {
            // tạo mới lại CustomerId
            customer.CustomerId = Guid.NewGuid();

            // chuỗi chứa tên các cột trong bảng
            var columnsName = string.Empty;

            // chuỗi chứa tên các value muốn insert vào
            var columnsParam = string.Empty;

            // lấy ra các properties có trong object customer
            var properties = customer.GetType().GetProperties();

            DynamicParameters parameters = new DynamicParameters();

            // duyệt từng property
            foreach (var prop in properties)
            {
                // lấy ra tên của property
                var propName = prop.Name;

                // lấy ra value của property
                var propValue = prop.GetValue(customer);

                // lấy ra kiểu của property
                var propType = prop.GetType();

                // gán giá trị vào chuỗi
                columnsName += $"{propName},";
                columnsParam += $"@{propName},";

                // add vào param
                parameters.Add($"@{propName}", propValue);
            }

            // cắt dấu phẩu cuối chuỗi
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

            // khai báo thông tin kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // khởi tạo đối tượng kết nối vào db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // thêm dữ liệu vào db
            var sqlQuery = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam})";
            var row = dbConnection.Execute(sqlQuery, param: parameters);

            // trả kết quả về cho client
            return row;
        }

        /// <summary>
        /// Lấy khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng muốn tìm kiếm</param>
        /// <returns>1 khách hàng trong db được tìm kiếm theo mã</returns>
        /// CreateBy: LQNhat(12/08/2021)
        public Customer GetCustomerByCode(string customerCode)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer WHERE CustomerCode = @customerCode";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerCode", customerCode);
            var customer = dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);

            // 4. trả về client
            return customer;
        }

        // Sửa thông tin khách hàng

        // Xóa thông tin khách hàng theo id khách hàng

        #endregion
    }
}
