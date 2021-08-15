using Dapper;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public int Add(Customer customer)
        {
            // tạo mới lại CustomerId
            customer.CustomerId = Guid.NewGuid();
            customer.CreatedDate = DateTime.UtcNow;

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
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // khởi tạo đối tượng kết nối vào db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // thêm dữ liệu vào db
            var sqlQuery = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam})";
            var rowEffect = dbConnection.Execute(sqlQuery, param: parameters);
            return rowEffect;
        }

        public int Delete(Guid customerId)
        {
            // kết nối vào db
            var connectionString = "Host = 47.241.69.179; " +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";
            // tạo đối tượng kết nối
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // xóa dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerIdParam", customerId);
            var sqlQuery = $"DELETE FROM Customer WHERE CustomerId = @customerIdParam";
            var result = dbConnection.Execute(sqlQuery, param: parameters);
            return result;
        }

        /// <summary>
        /// Lấy ra toàn bộ danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public IEnumerable<Customer> Get()
        {
            // kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer";
            var customers = dbConnection.Query<Customer>(sqlQuery);
            return customers;
        }

        /// <summary>
        /// Tìm kiếm khách hàng theo mã 
        /// </summary>
        /// <param name="customerId">ID khách hàng cần tìm kiếm</param>
        /// <param name="customerCode">Mã khách hàng cần tìm kiếm</param>
        /// <returns>Khách hàng cần tìm kiếm theo Id và mã</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        public Customer GetByCode(Guid? customerId, string customerCode)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer WHERE CustomerCode = @customerCode AND CustomerID <> @customerId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerCode", customerCode);
            parameters.Add("@customerId", customerId);
            var customer = dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);

            // 4. trả về client
            return customer;
        }

        /// <summary>
        /// Lấy ra thông tin của 1 khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id của khách hàng muốn lấy</param>
        /// <returns>Thông tin khách hàng muốn tìm theo Id</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public Customer GetById(Guid customerId)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer WHERE CustomerId = @customerId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerId", customerId);
            var customer = dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);
            return customer;
        }

        // <summary>
        /// Cập nhật thông tin khách hàng trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn cập nhật</param>
        /// <param name="customerUpdate">Dữ liệu muốn cập nhật</param>
        /// <returns>Số bản ghi khách hàng được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Update(Customer customer, Guid customerId)
        {
            // kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // chuỗi chứa tên các cột trong bảng
            var columnsName = string.Empty;

            // Lấy ra các properties có trong object
            var properties = customer.GetType().GetProperties();

            // khai báo param
            var dynamicParam = new DynamicParameters();

            // duyệt từng property trong mảng
            foreach (var prop in properties)
            {
                // lấy ra tên 
                var propName = prop.Name;

                // lấy ra value
                var propValue = prop.GetValue(customer);

                // gán giá trị vào chuỗi
                columnsName += $"{propName} = @{propName},";

                // gán giá trị vào param
                dynamicParam.Add($"@{propName}", propValue);
            }

            // cắt dấu phẩy cuối chuỗi
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);

            // sửa dữ liệu
            var sqlQuery = $"UPDATE Customer SET {columnsName} WHERE CustomerId = @customerId";
            DynamicParameters dynamicParameters = new DynamicParameters();
            // gán customerId vào param
            dynamicParameters.Add("@customerId", customerId);
            var result = dbConnection.Execute(sqlQuery, param: dynamicParam);
            return result;
        }


    }
}
