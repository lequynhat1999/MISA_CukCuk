using Dapper;
using Microsoft.Extensions.Configuration;
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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Constructor
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion



        #region Methods
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
        #endregion


    }
}
