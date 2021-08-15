using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Lấy ra tất cả thông tin của khách hàng trong database
        /// </summary>
        /// <returns>Danh sách khách hàng trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        IEnumerable<Customer> Get();

        /// <summary>
        /// Lấy ra thông tin của 1 khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id của khách hàng muốn lấy</param>
        /// <returns>Thông tin khách hàng muốn tìm theo Id</returns>
        /// CreateBy:LQNhat(09/08/2021)
        Customer GetById(Guid customerId);

        /// <summary>
        /// Thêm mới 1 khách hàng vào trong db
        /// </summary>
        /// <param name="customer">object khách hàng muốn thêm</param>
        /// <returns>Số bản ghi được thêm vào db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        int Add(Customer customer);

        /// <summary>
        /// Cập nhật thông tin khách hàng trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn cập nhật</param>
        /// <param name="customerUpdate">Dữ liệu muốn cập nhật</param>
        /// <returns>Số bản ghi khách hàng được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        int Update(Customer customer, Guid customerId);

        /// <summary>
        /// Xóa 1 khách hàng trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn xóa</param>
        /// <returns>Số bản ghi khách hàng được xóa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        int Delete(Guid customerId);

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
