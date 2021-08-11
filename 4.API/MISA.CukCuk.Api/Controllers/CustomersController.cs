using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore;
using MISA.CukCuk.Api.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// Lấy ra tất cả thông tin của khách hàng trong database
        /// </summary>
        /// <returns>Danh sách khách hàng trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customerService = new CustomerService();
            var customers = customerService.GetCustomers();
            return Ok(customers);

            //// kết nối db
            //var connectionString = "Host = 47.241.69.179;" +
            //     "Database = MISA.CukCuk_Demo_NVMANH;" +
            //     "User Id = dev;" +
            //     "Password = 12345678;";

            //// tạo đối tượng kết nối db
            //IDbConnection dbConnection = new MySqlConnection(connectionString);

            //// lấy dữ liệu
            //var sqlQuery = "SELECT * FROM Customer";
            //var customers = dbConnection.Query<object>(sqlQuery);

            //// trả về client
            //var res = StatusCode(200, customers);
            //return res;
        }

        /// <summary>
        /// Lấy ra thông tin của 1 khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id của khách hàng muốn lấy</param>
        /// <returns>Thông tin khách hàng muốn tìm theo Id</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(Guid customerId)
        {
            var customerService = new CustomerService();
            var customer = customerService.GetCustomerById(customerId);
            return Ok(customer);

            // trả dữ liệu về cho client
            //var customer = GetCustomerForPut(customerId);
            //var response = StatusCode(200, customer);
            //return response;
        }

        /// <summary>
        /// Trả về 1 object khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn lấy</param>
        /// <returns>Object khách hàng tìm theo Id</returns>
        /// CreateBy:LQNhat(09/08/2021)
        private Customer GetCustomerForPut(Guid customerId)
        {
            // kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // lấy dữ liệu
            var sqlQuery = $"SELECT * FROM Customer WHERE CustomerId = @CustomerIdParam ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerIdParam", customerId);
            var customer = dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);

            // trả dữ liệu về cho server
            return customer;
        }

        /// <summary>
        /// Thêm mới 1 khách hàng vào trong db
        /// </summary>
        /// <param name="customer">object khách hàng muốn thêm</param>
        /// <returns>Số bản ghi được thêm vào db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
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

            // Validate dữ liệu
            // Check trường mã bắt buộc nhập:
            var customerCode = customer.CustomerCode;
            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "CustomerCode", msg = "Mã khách hàng không được phép để trống" },
                    userMsg = "Mã khách hàng không được phép để trống",
                    Code = 999,
                };
                return BadRequest(msg);
            }

            // check trùng mã
            var sqlCommand = "SELECT * FROM Customer";
            var customers = dbConnection.Query<object>(sqlCommand);


            // thêm dữ liệu vào db
            var sqlQuery = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam})";
            var row = dbConnection.Execute(sqlQuery, param: parameters);

            // trả kết quả về cho client
            if (row > 0)
            {
                return Created("created", customer);
            }
            else
            {
                return NoContent();
            }

        }

        /// <summary>
        /// Cập nhật thông tin khách hàng trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn cập nhật</param>
        /// <param name="customerUpdate">Dữ liệu muốn cập nhật</param>
        /// <returns>Số bản ghi khách hàng được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(Guid customerId, Customer customerUpdate)
        {
            // lấy ra object customer muốn sửa theo Id
            //var customer = GetCustomerForPut(customerId);

            // kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // chuỗi chứa tên các cột trong bảng
            var columnsName = string.Empty;

            // Lấy ra các properties có trong object
            var properties = customerUpdate.GetType().GetProperties();

            // khai báo param
            var dynamicParam = new DynamicParameters();

            // duyệt từng property trong mảng
            foreach (var prop in properties)
            {
                // lấy ra tên 
                var propName = prop.Name;

                // lấy ra value
                var propValue = prop.GetValue(customerUpdate);

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

            // trả kết quả về cho client
            var respone = StatusCode(200, result);
            return respone;
        }

        /// <summary>
        /// Xóa 1 khách hàng trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn xóa</param>
        /// <returns>Số bản ghi khách hàng được xóa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(Guid customerId)
        {
            // kết nối vào db
            var connectionString = "Host = 47.241.69.179; " +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";
            // tạo đối tượng kết nối
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // xóa dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerIdParam", customerId);
            var sqlQuery = $"DELETE FROM Customer WHERE CustomerId = @customerIdParam";
            var result = dbConnection.Execute(sqlQuery, param: parameters);

            // trả về kết quả cho client
            var res = StatusCode(200, result);
            return res;
        }

    }


}
