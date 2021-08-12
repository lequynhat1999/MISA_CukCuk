using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore;
using MISA.Entity;
using MISA.Infrastructure.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
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
            //var customerService = new CustomerService();
            //var customers = customerService.GetCustomers();
            //return Ok(customers);

            try
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
                var customers = dbConnection.Query<object>(sqlQuery);

                // trả về client
                if (customers.Count() > 0)
                {
                    return StatusCode(200, customers);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVn.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
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
            //var customerService = new CustomerService();
            //var customer = customerService.GetCustomerById(customerId);
            //return Ok(customer);

            try
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
                var customer = dbConnection.QueryFirstOrDefault<object>(sqlQuery, param: parameters);

                // 4. trả về client
                if (customer != null)
                {
                    return StatusCode(200, customer);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVn.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }

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
            //var customerService = new CustomerService();
            //var serviceResult = customerService.InsertCustomer(customer);
            //if (serviceResult.MISAErrorCode == MISAEnum.NotValid)
            //{
            //    return BadRequest(serviceResult.Data);
            //}
            //if (serviceResult.MISAErrorCode == MISAEnum.IsValid && (int)serviceResult.Data > 0)
            //{
            //    return Created("created", customer);
            //}
            //else
            //{
            //    return NoContent();
            //}

            try
            {
                // Validate dữ liệu
                // Check trường mã bắt buộc nhập:
                var customerCode = customer.CustomerCode;
                if (string.IsNullOrEmpty(customerCode))
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVn.Dev_ErrorMsg__Null_CustomerCode,
                        userMsg = Properties.ResourceVn.User_ErrorMsg_Null_CustomerCode,
                    };
                    return StatusCode(400, msg);
                }

                // check email
                var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                var isMatch = Regex.IsMatch(customer.Email, emailFormat, RegexOptions.IgnoreCase);
                if (isMatch == false)
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVn.Dev_ErrorMsg_FormatEmail,
                        userMsg = Properties.ResourceVn.User_ErrorMsg_FormatEmail,
                    };
                    return StatusCode(400, msg);
                }

                // check trùng mã
                var row = GetCustomerByCode(customerCode);
                if (row != null)
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVn.Dev_ErrorMsg_Check_CustomerCode,
                        userMsg = Properties.ResourceVn.User_ErrorMsg_Check_CustomerCode,
                    };
                    return StatusCode(400, msg);
                }

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
                var rowAffect = dbConnection.Execute(sqlQuery, param: parameters);

                // trả kết quả về cho client
                if (rowAffect > 0)
                {
                    return StatusCode(201, customer);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVn.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
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
            try
            {
                // Validate dữ liệu
                // Check trường mã bắt buộc nhập:
                var customerCode = customerUpdate.CustomerCode;
                if (string.IsNullOrEmpty(customerCode))
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVn.Dev_ErrorMsg__Null_CustomerCode,
                        userMsg = Properties.ResourceVn.User_ErrorMsg_Null_CustomerCode,
                    };
                    return StatusCode(400, msg);
                }

                // check email
                var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                var isMatch = Regex.IsMatch(customerUpdate.Email, emailFormat, RegexOptions.IgnoreCase);
                if (isMatch == false)
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVn.Dev_ErrorMsg_FormatEmail,
                        userMsg = Properties.ResourceVn.User_ErrorMsg_FormatEmail,
                    };
                    return StatusCode(400, msg);
                }

                // check trùng mã
                var row = GetCustomerByCode(customerCode);
                if (row != null)
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVn.Dev_ErrorMsg_Check_CustomerCode,
                        userMsg = Properties.ResourceVn.User_ErrorMsg_Check_CustomerCode,
                    };
                    return StatusCode(400, msg);
                }

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
                if (result > 0)
                {
                    return StatusCode(200, customerUpdate);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVn.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
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
            try
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
                if (result > 0)
                {
                    return StatusCode(200, result);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVn.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
        }

        /// <summary>
        /// Lấy khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng muốn tìm kiếm</param>
        /// <returns>1 khách hàng trong db được tìm kiếm theo mã</returns>
        /// CreateBy: LQNhat(12/08/2021)
        private Customer GetCustomerByCode(string customerCode)
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

    }


}
