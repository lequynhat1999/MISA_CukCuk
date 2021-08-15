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
    public class PositionRepository : IPositionRepository
    {
        /// <summary>
        /// Thêm mới 1 vị trí vào trong db
        /// </summary>
        /// <param name="position">dữ liệu vị trí thêm mới</param>
        /// <returns>Số bản ghi được thêm trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Add(Position position)
        {
            position.PositionId = Guid.NewGuid();
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
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. thêm dữ liệu
            var sqlQuery = $"INSERT INTO `Position`({columnsName}) VALUES({columnsValue})";
            var result = dbConnection.Execute(sqlQuery, param: param);
            return result;
        }

        /// <summary>
        /// Xóa 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">Id vị trí muốn xóa</param>
        /// <returns>Số bản ghi vừa xóa trong db</returns>
        /// CreateBy: LQNhat(09/08/2021)
        public int Delete(Guid positionId)
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. xóa dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@positionId", positionId);
            var sqlQuery = "DELETE FROM Position WHERE PositionId = @positionId";
            var result = dbConnection.Execute(sqlQuery, parameters);
            return result;
        }

        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về vị trí trong db
        /// </summary>
        /// <returns>Danh sách vị trí</returns>
        /// CreateBy: LQNhat(9/8/2021)
        public IEnumerable<Position> Get()
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Position";
            var positions = dbConnection.Query<Position>(sqlQuery);
            return positions;
        }

        /// <summary>
        /// lấy ra dữ liệu về 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí</param>
        /// <returns>Vị trí tìm theo Id</returns>
        /// CreateBy: LQNhat(9/8/2021)
        public Position GetById(Guid positionId)
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu theo id
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@positionId", positionId);
            var sqlQuery = "SELECT * FROM Position WHERE PositionId = @positionId";
            var position = dbConnection.QueryFirstOrDefault<Position>(sqlQuery, param: parameters);
            return position;
        }

        /// <summary>
        /// Sửa thông tin 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí muốn sửa</param>
        /// <param name="position">dữ liệu vị trí muốn sửa</param>
        /// <returns>Số bản ghi được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Update(Position position, Guid positionId)
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
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. sửa dữ liệu
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@positionId", positionId);
            var sqlQuery = $"UPDATE `Position` SET {columnsName} WHERE PositionId = @positionId";
            var result = dbConnection.Execute(sqlQuery, param: param);
            return result;
        }
    }
}
