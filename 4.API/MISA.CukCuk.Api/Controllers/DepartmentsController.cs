using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về phòng ban trong db
        /// </summary>
        /// <returns></returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet]
        public IActionResult GetDepartment()
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Department";
            var departments = dbConnection.Query<object>(sqlQuery);

            // 4. trả kết quả về cho client
            var res = StatusCode(200, departments);
            return res;

        }

        /// <summary>
        /// lấy ra dữ liệu về 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban</param>
        /// <returns></returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet("{departmentId}")]
        public IActionResult GetDepartmentById(Guid departmentId)
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu theo id
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@departmentId", departmentId);
            var sqlQuery = "SELECT * FROM Department WHERE DepartmentId = @departmentId";
            var department = dbConnection.QueryFirstOrDefault<object>(sqlQuery,param:parameters);

            // 4. trả kết quả về cho client
            var res = StatusCode(200, department);
            return res;
        }

        /// <summary>
        /// Thêm mới 1 phòng ban vào trong db
        /// </summary>
        /// <param name="department">dữ liệu phòng ban thêm mới</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertDepartment(Department department)
        {
            var columnsName = string.Empty;
            var columnsValue = string.Empty;
            var properties = department.GetType().GetProperties();
            var param = new DynamicParameters();

            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(department);

                param.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsValue += $"@{propName},";
            }

            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsValue = columnsValue.Remove(columnsValue.Length - 1, 1);

            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. thêm dữ liệu
            var sqlQuery = $"INSERT INTO Department({columnsName}) VALUES({columnsValue})";
            var result = dbConnection.Execute(sqlQuery, param: param);

            // 4. trả kết quả về client
            var res = StatusCode(200, result);
            return res;

        }

        /// <summary>
        /// Sửa thông tin 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban muốn sửa</param>
        /// <param name="department">dữ liệu phòng ban muốn sửa</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{departmentId}")]
        public IActionResult UpdateDepartment(Guid departmentId, Department department)
        {
            var columnsName = string.Empty;
            var param = new DynamicParameters();
            var properties = department.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(department);

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

            // 3. sửa dữ liệu
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@departmentId", departmentId);
            var sqlQuery = $"UPDATE Department SET {columnsName} WHERE DepartmentId = @departmentId";
            var result = dbConnection.Execute(sqlQuery, param: param);

            // 4. trả về cho client
            var res = StatusCode(200, result);
            return res;
        }

        /// <summary>
        /// Xóa 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">Id phòng ban muốn xóa</param>
        /// <returns></returns>
        /// CreateBy: LQNhat(09/08/2021)
        [HttpDelete("{departmentId}")]
        public IActionResult DeleteDepartment(Guid departmentId)
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
            parameters.Add("@departmentId", departmentId);
            var sqlQuery = "DELETE FROM Department WHERE DepartmentId = @departmentId";
            var result = dbConnection.Execute(sqlQuery, parameters);

            // 4. trả kết quả về client
            var res = StatusCode(200, result);
            return res;
        }
    }
}
