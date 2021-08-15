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
    public class DepartmentRepository : IDepartmentRepository
    {
        /// <summary>
        /// Thêm mới 1 phòng ban vào trong db
        /// </summary>
        /// <param name="department">dữ liệu phòng ban thêm mới</param>
        /// <returns>Số bản ghi được thêm vào db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Add(Department department)
        {
            department.DepartmentId = Guid.NewGuid();
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
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. thêm dữ liệu
            var sqlQuery = $"INSERT INTO Department({columnsName}) VALUES({columnsValue})";
            var result = dbConnection.Execute(sqlQuery, param: param);
            return result;
        }

        /// <summary>
        /// Xóa 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">Id phòng ban muốn xóa</param>
        /// <returns>Số bản ghi đã xóa trong db</returns>
        /// CreateBy: LQNhat(09/08/2021)
        public int Delete(Guid departmentId)
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
            parameters.Add("@departmentId", departmentId);
            var sqlQuery = "DELETE FROM Department WHERE DepartmentId = @departmentId";
            var result = dbConnection.Execute(sqlQuery, parameters);
            return result;
        }

        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về phòng ban trong db
        /// </summary>
        /// <returns>Danh sách phòng ban</returns>
        /// CreateBy: LQNhat(9/8/2021)
        public IEnumerable<Department> Get()
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. Lấy dữ liệu
            var sqlQuery = "SELECT * FROM Department";
            var departments = dbConnection.Query<Department>(sqlQuery);
            return departments;
        }

        /// <summary>
        /// lấy ra dữ liệu về 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban</param>
        /// <returns>Phòng ban tìm kiếm theo Id</returns>
        /// CreateBy: LQNhat(9/8/2021)
        public Department GetById(Guid departmentId)
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
            parameters.Add("@departmentId", departmentId);
            var sqlQuery = "SELECT * FROM Department WHERE DepartmentId = @departmentId";
            var department = dbConnection.QueryFirstOrDefault<Department>(sqlQuery, param: parameters);
            return department;
        }

        /// <summary>
        /// Sửa thông tin 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban muốn sửa</param>
        /// <param name="department">dữ liệu phòng ban muốn sửa</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Update(Department department, Guid departmentId)
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
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. sửa dữ liệu
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@departmentId", departmentId);
            var sqlQuery = $"UPDATE Department SET {columnsName} WHERE DepartmentId = @departmentId";
            var result = dbConnection.Execute(sqlQuery, param: param);
            return result;
        }
    }
}
