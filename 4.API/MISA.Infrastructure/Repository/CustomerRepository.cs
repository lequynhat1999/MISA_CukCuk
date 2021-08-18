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
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        #region Constructor
        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy ra khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Khách hàng được lọc theo điều kiện</returns>
        public Customer GetByCode(Guid? customerId, string customerCode)
        {
            // 3. lấy dữ liệu
            var sqlQuery = "SELECT * FROM Customer WHERE CustomerCode = @customerCode AND CustomerID <> @customerId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerCode", customerCode);
            parameters.Add("@customerId", customerId.ToString());
            var customer = _dbConnection.QueryFirstOrDefault<Customer>(sqlQuery, param: parameters);

            // 4. trả về client
            return customer;
        }
        #endregion
    }
}
