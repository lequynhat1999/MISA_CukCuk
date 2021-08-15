using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Thêm mới khách hàng vào trong db
        /// </summary>
        /// <param name="customer">Dữ liệu khách hàng muốn thêm</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Add(Customer customer);

        /// <summary>
        /// Sửa 1 khách hàng trong db
        /// </summary>
        /// <param name="customer">Dữ liệu khách hàng muốn sửa</param>
        /// <param name="customerId">Id khách hàng muốn sửa</param>
        /// <returns>ServiceResult - kết quả xử lý nghiệp vụ</returns>
        /// CreateBy: LQNHAT(15/08/2021)
        ServiceResult Update(Customer customer, Guid customerId);
    }
}
