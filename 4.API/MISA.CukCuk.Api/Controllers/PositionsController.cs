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
    public class PositionsController : ControllerBase
    {
        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về vị trí trong db
        /// </summary>
        /// <returns></returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet]
        public IActionResult GetPosition()
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISA.CukCuk_Demo_NVMANH;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Position";
            var positions = dbConnection.Query<object>(sqlQuery);

            // 4. trả kết quả về cho client
            var res = StatusCode(200, positions);
            return res;

        }

        /// <summary>
        /// lấy ra dữ liệu về 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí</param>
        /// <returns></returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet("{positionId}")]
        public IActionResult GetPositionById(Guid positionId)
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
            parameters.Add("@positionId", positionId);
            var sqlQuery = "SELECT * FROM Position WHERE PositionId = @positionId";
            var position = dbConnection.QueryFirstOrDefault<object>(sqlQuery, param: parameters);

            // 4. trả kết quả về cho client
            var res = StatusCode(200, position);
            return res;
        }

        /// <summary>
        /// Thêm mới 1 vị trí vào trong db
        /// </summary>
        /// <param name="position">dữ liệu vị trí thêm mới</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertPosition(Position position)
        {
            var columnsName = string.Empty;
            var columnsValue = string.Empty;
            var properties = position.GetType().GetProperties();
            var param = new DynamicParameters();

            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(position);

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
            var sqlQuery = $"INSERT INTO `Position`({columnsName}) VALUES({columnsValue})";
            var result = dbConnection.Execute(sqlQuery, param: param);

            // 4. trả kết quả về client
            var res = StatusCode(200, result);
            return res;

        }

        /// <summary>
        /// Sửa thông tin 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí muốn sửa</param>
        /// <param name="position">dữ liệu vị trí muốn sửa</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{positionId}")]
        public IActionResult UpdatePosition(Guid positionId, Position position)
        {
            var columnsName = string.Empty;
            var param = new DynamicParameters();
            var properties = position.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(position);

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
            dynamicParameters.Add("@positionId", positionId);
            var sqlQuery = $"UPDATE `Position` SET {columnsName} WHERE PositionId = @positionId";
            var result = dbConnection.Execute(sqlQuery, param: param);

            // 4. trả về cho client
            var res = StatusCode(200, result);
            return res;
        }

        /// <summary>
        /// Xóa 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">Id vị trí muốn xóa</param>
        /// <returns></returns>
        /// CreateBy: LQNhat(09/08/2021)
        [HttpDelete("{positionId}")]
        public IActionResult DeletePosition(Guid positionId)
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
            parameters.Add("@positionId", positionId);
            var sqlQuery = "DELETE FROM Position WHERE PositionId = @positionId";
            var result = dbConnection.Execute(sqlQuery, parameters);

            // 4. trả kết quả về client
            var res = StatusCode(200, result);
            return res;
        }
    }
}
