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
    public class EmployeeRepository : IEmployeeRepository
    {
        /// <summary>
        /// Thêm 1 nhân viên vào db
        /// </summary>
        /// <param name="employee">dữ liệu về nhân viên muốn thêm</param>
        /// <returns>Số bản ghi được thêm vào trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Add(Employee employee)
        {
            // sinh id mới
            employee.EmployeeId = Guid.NewGuid();

            // ngày tạo mới
            employee.CreatedDate = DateTime.UtcNow;
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
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            // 3. thêm dữ liệu
            var sqlQuery = $"INSERT INTO Employee({columnsName}) VALUES({columnsParam}) ";
            var result = dbConnection.Execute(sqlQuery, param: param);
            return result;
        }

        // <summary>
        /// Xóa 1 nhân viên trong db
        /// </summary>
        /// <param name="employeeId">Id của nhân viên</param>
        /// <returns>Số dòng được xóa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Delete(Guid? employeeId)
        {
            // 1. kết nối db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. xóa dữ liệu
            var result = dbConnection.Execute("Proc_DeleteEmployee", new { _employeeId = employeeId }, commandType: CommandType.StoredProcedure);
            return result;
        }

        /// <summary>
        /// Lấy ra dữ liệu của toàn bộ nhân viên trong db
        /// </summary>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        public IEnumerable<Employee> Get()
        {
            // 1. kết vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. Tạo đối tượng kết nối vào db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. Lấy dữ liệu
            var employees = dbConnection.Query<Employee>("Proc_GetEmployees", commandType: CommandType.StoredProcedure);
            return employees;
        }

        /// <summary>
        /// Lấy ra 1 nhân viên theo mã nhân viên
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>1 nhân viên được tìm kiếm theo mã</returns>
        /// CreateBy:LQNhat(10/08/2021)
        public Employee GetByCode(string employeeCode, Guid? employeeId)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Employee WHERE EmployeeCode = @employeeCode AND EmployeeID <> @employeeId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@employeeCode", employeeCode);
            parameters.Add("@employeeId", employeeId.ToString());
            var employee = dbConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: parameters);

            // 4. trả về client
            return employee;
        }

        /// <summary>
        /// Lấy ra dữ liệu 1 nhân viên theo Id
        /// </summary>
        /// <param name="employeeId">Id nhân viên muốn lấy ra</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        public Employee GetById(Guid employeeId)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var employee = dbConnection.QueryFirstOrDefault<Employee>("Proc_GetEmployeeById", new { EmployeeId = employeeId }, commandType: CommandType.StoredProcedure);
            return employee;
        }

        /// <summary>
        /// Lọc danh sách nhân viên theo các tiêu chí và phân trang
        /// </summary>
        /// <param name="pageIndex">Index của trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi hiển thị trên 1 trang</param>
        /// <param name="positionId">Id của vị trí cần tìm kiếm</param>
        /// <param name="departmentId">Id của phòng ban cần tìm kiếm</param>
        /// <param name="keysearch">Mã nhân viên, Họ và tên, SĐT cần tìm kiếm</param>
        /// <returns>Danh sách các bản ghi theo điều kiện lọc</returns>
        /// CreateBy: LQNHAT(14/08/2021)
        public IEnumerable<Employee> GetByPaging(int pageIndex, int pageSize, string positionId, string departmentId, string keysearch)
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu
            var employees = dbConnection.Query<Employee>("Proc_GetEmployeesPaging",
                new
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PositionId = positionId,
                    DepartmentId = departmentId,
                    Keysearch = keysearch
                }, commandType: CommandType.StoredProcedure);
            return employees;
        }

        /// <summary>
        /// Tự sinh mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới dạng string</returns>
        /// CreateBy: LQNHAT(13/08/2021)
        public string NewCode()
        {
            // 1. kết nối vào db
            var connectionString = "Host = 47.241.69.179;" +
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. lấy dữ liệu

            // lấy ra mã nhân viên trên cùng
            var topEmployeeCode = dbConnection.QueryFirstOrDefault<string>("Proc_GetTopEmployeeCode", commandType: CommandType.StoredProcedure);
            var valueNewEmployeeCode = 0;
            if (topEmployeeCode != null)
            {
                // cắt chuỗi
                var valueEmployeeCode = int.Parse(topEmployeeCode.ToString().Split("-")[1]);
                // gán giá trị
                if (valueNewEmployeeCode < valueEmployeeCode)
                {
                    valueNewEmployeeCode = valueEmployeeCode;
                }
            }
            else
            {
                topEmployeeCode = "NV-0";
                // cắt chuỗi
                var valueEmployeeCode = int.Parse(topEmployeeCode.ToString().Split("-")[1]);
                // gán giá trị
                if (valueNewEmployeeCode < valueEmployeeCode)
                {
                    valueNewEmployeeCode = valueEmployeeCode;
                }
            }
            string newEmployeeCode = "NV-" + (valueNewEmployeeCode + 1);
            return newEmployeeCode;
        }

        /// <summary>
        /// Sửa thông tin 1 nhân viên trong db
        /// </summary>
        /// <param name="employeeId">Id của nhân viên muốn sửa</param>
        /// <param name="employee">Dữ liệu nhân viên muốn sửa</param>
        /// <returns>Số bản ghi được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Update(Employee employee, Guid employeeId)
        {
            // ngày chỉnh sửa
            employee.ModifiedDate = DateTime.UtcNow;
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
                 "Database = MISACukCuk_MF949_LQNHAT;" +
                 "User Id = dev;" +
                 "Password = 12345678;";

            // 2. tạo đối tượng kết nối db
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3. sửa dữ liệu
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@employeeId", employeeId);
            var sqlQuery = $"UPDATE Employee SET {columnsName} WHERE EmployeeId = @employeeId";
            var result = dbConnection.Execute(sqlQuery, param: param);
            return result;
        }
    }
}
