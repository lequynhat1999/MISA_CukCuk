using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        /// <summary>
        /// Lấy khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn tìm kiếm</param>◘
        /// <param name="customerCode">Mã khách hàng muốn tìm kiếm</param>
        /// <returns>1 khách hàng trong db được tìm kiếm theo mã</returns>
        /// CreateBy: LQNhat(12/08/2021)
        Customer GetByCode(Guid? customerId, string customerCode);
    }
}
