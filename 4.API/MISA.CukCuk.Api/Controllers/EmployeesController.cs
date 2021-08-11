using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;
using Dapper;
using MISA.CukCuk.Api.Model;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        /// <summary>
        /// Lấy ra dữ liệu của toàn bộ nhân viên trong db
        /// </summary>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet]
        public IActionResult GetEmployees()
        {
            // 1. kết vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. Tạo đối tượng kết nối vào db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Employee";
            var employees = dbConnection.Query<object>(sqlQuery);

            // 4. Trả về Client
            var res = StatusCode(200, employees);
            return res;
        }

        /// <summary>
        /// Lấy ra dữ liệu 1 nhân viên theo Id
        /// </summary>
        /// <param name="employeeId">Id nhân viên muốn lấy ra</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Employee WHERE EmployeeId = @employeeId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@employeeId", employeeId);
            var employee = dbConnection.QueryFirstOrDefault<object>(sqlQuery, param: parameters);

            // 4. trả về client
            var res = StatusCode(200, employee);
            return res;
        }

        /// <summary>
        /// Phân trang cho danh sách nhân viên
        /// </summary>
        /// <param name="pageSize">Số bản ghi trong 1 trang</param>
        /// <param name="pageIndex">Index của trang</param>
        /// <returns>Số bản ghi trong 1 trang</returns>
        [HttpGet("paging/{pageSize}/{pageIndex}")]
        public IActionResult PagingEmployee(int pageSize, int pageIndex)
        {
            // 1. kết db
            var connetionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng connect db
            IDbConnection dbConnection = new MySqlConnection(connetionString);

            // 3. lấy dữ liệu

            // gán value cho OFFSET và LIMIT
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@offsetParam", (pageIndex - 1) * pageSize);
            dynamicParameters.Add("@limitParam", pageSize);

            var sqlQuery = "SELECT * FROM Employee ORDER BY CreatedDate DESC LIMIT @limitParam OFFSET @offsetParam";
            //var sqlQuery = "SELECT * FROM Employee ORDER BY CreatedDate DESC LIMIT @limitParam";
            var employees = dbConnection.Query<object>(sqlQuery, param: dynamicParameters);

            // 4. trả về cho client
            var res = StatusCode(200, employees);
            return res;
        }

        ///{input}/{departmentId}/{positionId}
        [HttpGet("filter/{departmentId}")]
        public IActionResult GetEmployeeByFilter(string input, Guid? departmentId, Guid? positionId)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@employeeCode", input);
            dynamicParameters.Add("@fullName", input);
            dynamicParameters.Add("@phoneNumber", input);
            dynamicParameters.Add("@departmentId", departmentId);
            dynamicParameters.Add("@positionId", positionId);

            //var sqlQuery = "SELECT * FROM Employee WHERE (EmployeeCode = @employeeCode OR Fullname = @fullName" +
            //    " OR PhoneNumber = @phoneNumber) " +
            //    "AND DepartmentId = @departmentId AND PositionId = @positionId ";
            //('%', @employeeCode, '%')

            //var sqlQuery = "SELECT * FROM Employee WHERE EmployeeCode like @employeeCode " +
            //    "OR Fullname like @fullName " +
            //    "OR PhoneNumber like @phoneNumber " ;
            var sqlQuery = "SELECT * FROM Employee WHERE DepartmentId = @departmentId";

            var employees = dbConnection.Query<object>(sqlQuery, param: dynamicParameters);


            // 4. trả kết quả về cho client
            var res = StatusCode(200, employees);
            return res;
        }

        /// <summary>
        /// Thêm 1 nhân viên vào db
        /// </summary>
        /// <param name="employee">dữ liệu về nhân viên muốn thêm</param>
        /// <returns>Số bản ghi được thêm vào trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertEmployee(Employee employee)
        {
            // sinh id mới
            employee.EmployeeId = Guid.NewGuid();

            // chuỗi chứa tên cột
            var columnsName = string.Empty;

            // chuỗi chứa param
            var columnsParam = string.Empty;

            var properties = employee.GetType().GetProperties();

            var param = new DynamicParameters();

            foreach (var prop in properties)
            {
                var propName = prop.Name;

                var propValue = prop.GetValue(employee);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";

                param.Add($"@{propName}", propValue);

            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // Validate dữ liệu
            // Check trường mã bắt buộc nhập:
            var employeeCode = employee.EmployeeCode;
            if (string.IsNullOrEmpty(employeeCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeCode", msg = "Mã Nhân viên không được phép để trống" },
                    userMsg = "Mã Nhân viên không được phép để trống",
                    Code = 999,
                };
                return BadRequest(msg);
            }

            // check trùng mã
            var row = GetEmployeeByCode(employeeCode);

            if (row != null)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeCode", msg = "Mã Nhân viên đã tồn tại" },
                    userMsg = "Mã Nhân viên đã tồn tại",
                    Code = 998,
                };
                return BadRequest(msg);
            }



            // 3. thêm dữ liệu
            var sqlQuery = $"INSERT INTO Employee({columnsName}) VALUES({columnsParam}) ";
            var result = dbConnection.Execute(sqlQuery, param: param);
            // 4. trả về client
            var res = StatusCode(200, result);
            return res;

        }

        /// <summary>
        /// Sửa thông tin 1 nhân viên trong db
        /// </summary>
        /// <param name="employeeId">Id của nhân viên muốn sửa</param>
        /// <param name="employee">Dữ liệu nhân viên muốn sửa</param>
        /// <returns>Số bản ghi được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Guid employeeId, Employee employee)
        {
            var columnsName = string.Empty;
            var param = new DynamicParameters();
            var properties = employee.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(employee);

                columnsName += $"{propName} = @{propName},";
                param.Add($"@{propName}", propValue);
            }

            columnsName = columnsName.Remove(columnsName.Length - 1, 1);

            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // Validate dữ liệu
            // Check trường mã bắt buộc nhập:
            var employeeCode = employee.EmployeeCode;
            if (string.IsNullOrEmpty(employeeCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeCode", msg = "Mã Nhân viên không được phép để trống" },
                    userMsg = "Mã Nhân viên không được phép để trống",
                    Code = 999,
                };
                return BadRequest(msg);
            }

            // check trùng mã
            var row = GetEmployeeByCode(employeeCode);
            if (row != null)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "EmployeeCode", msg = "Mã Nhân viên đã tồn tại" },
                    userMsg = "Mã Nhân viên đã tồn tại",
                    Code = 998,
                };
                return BadRequest(msg);
            }

            // 3. sửa dữ liệu
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@employeeId", employeeId);
            var sqlQuery = $"UPDATE Employee SET {columnsName} WHERE EmployeeId = @employeeId";
            var result = dbConnection.Execute(sqlQuery, param: param);

            // 4. trả về cho client
            var res = StatusCode(200, result);
            return res;

        }

        /// <summary>
        /// Xóa 1 nhân viên trong db
        /// </summary>
        /// <param name="employeeId">Id của nhân viên</param>
        /// <returns>Số dòng được xóa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(Guid employeeId)
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. xóa dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@employeeId", employeeId);
            var sqlQuery = "DELETE FROM Employee WHERE EmployeeId = @employeeId";
            var result = dbConnection.Execute(sqlQuery, parameters);

            // 4. trả kết quả về client
            var res = StatusCode(200, result);
            return res;
        }

        /// <summary>
        /// Lấy ra 1 nhân viên theo mã nhân viên
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>1 nhân viên được tìm kiếm theo mã</returns>
        /// CreateBy:LQNhat(10/08/2021)
        private Employee GetEmployeeByCode(string employeeCode)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Employee WHERE EmployeeCode = @employeeCode";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@employeeCode", employeeCode);
            var employee = dbConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: parameters);

            // 4. trả về client
            return employee;
        }

    }
}
