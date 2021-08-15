using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MISA.Core.MISAEnum;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerService _customerService;
        ICustomerRepository _customerRepository;
        public CustomersController(ICustomerService customerService, ICustomerRepository customerRepository)
        {
            _customerService = customerService;
            _customerRepository = customerRepository;
        }

        #region Methods

        /// <summary>
        /// Lấy ra tất cả thông tin của khách hàng trong database
        /// </summary>
        /// <returns>Danh sách khách hàng trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet]
        public IActionResult GetCustomers()
        {
            try
            {
                var customers = _customerRepository.Get();
                // trả về client
                if (customers.Count() > 0)
                {
                    return StatusCode(200, customers);
                }
                else
                {
                    var msg = new
                    {
                        userMsg = Properties.ResourceVnCustomer.User_ErrorMsg_NoContent,
                    };
                    return StatusCode(204, msg);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnCustomer.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
        }

        /// <summary>
        /// Lấy ra thông tin của 1 khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id của khách hàng muốn lấy</param>
        /// <returns>Thông tin khách hàng muốn tìm theo Id</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(Guid customerId)
        {
            try
            {
                var customer = _customerRepository.GetById(customerId);
                // 4. trả về client
                if (customer != null)
                {
                    return StatusCode(200, customer);
                }
                else
                {
                    var msg = new
                    {
                        userMsg = Properties.ResourceVnCustomer.User_ErrorMsg_NoContent,
                    };
                    return StatusCode(204, msg);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnCustomer.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }

        }

        /// <summary>
        /// Thêm mới 1 khách hàng vào trong db
        /// </summary>
        /// <param name="customer">object khách hàng muốn thêm</param>
        /// <returns>Số bản ghi được thêm vào db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
            try
            {
                // trả kết quả về cho client
                var serviceResult = _customerService.Add(customer);
                if (serviceResult.MISACode == EnumServiceResult.Created)
                {
                    return StatusCode(201, serviceResult.Data);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnCustomer.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }

        }

        /// <summary>
        /// Cập nhật thông tin khách hàng trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn cập nhật</param>
        /// <param name="customerUpdate">Dữ liệu muốn cập nhật</param>
        /// <returns>Số bản ghi khách hàng được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(Guid customerId, Customer customerUpdate)
        {
            try
            {
                // Validate dữ liệu
                // Check trường mã bắt buộc nhập:
                var customerCode = customerUpdate.CustomerCode;
                if (string.IsNullOrEmpty(customerCode))
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVnCustomer.Dev_ErrorMsg__Null_CustomerCode,
                        userMsg = Properties.ResourceVnCustomer.User_ErrorMsg_Null_CustomerCode,
                    };
                    return StatusCode(400, msg);
                }

                // Check họ và tên khách hàng bắt buộc nhập
                var customerFullName = customerUpdate.FullName;
                if (string.IsNullOrEmpty(customerFullName))
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVnCustomer.Dev_ErrorMsg_Null_FullName,
                        userMsg = Properties.ResourceVnCustomer.User_ErrorMsg_Null_FullName,
                    };
                    return StatusCode(400, msg);
                }

                // Check Email bắt buộc nhập
                var customerEmail = customerUpdate.Email;
                if (string.IsNullOrEmpty(customerEmail))
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVnCustomer.Dev_ErrorMsg_Null_Email,
                        userMsg = Properties.ResourceVnCustomer.User_ErrorMsg_Null_Email,
                    };
                    return StatusCode(400, msg);
                }

                // Check SĐT bắt buộc nhập
                var customerPhoneNumber = customerUpdate.PhoneNumber;
                if (string.IsNullOrEmpty(customerPhoneNumber))
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVnCustomer.Dev_ErrorMsg_Null_PhoneNumber,
                        userMsg = Properties.ResourceVnCustomer.User_ErrorMsg_Null_PhoneNumber,
                    };
                    return StatusCode(400, msg);
                }

                // check email
                var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                var isMatch = Regex.IsMatch(customerUpdate.Email, emailFormat, RegexOptions.IgnoreCase);
                if (isMatch == false)
                {
                    var msg = new
                    {
                        devMsg = Properties.ResourceVnCustomer.Dev_ErrorMsg_FormatEmail,
                        userMsg = Properties.ResourceVnCustomer.User_ErrorMsg_FormatEmail,
                    };
                    return StatusCode(400, msg);
                }

                // trả kết quả về cho client
                var serviceResult = _customerService.Update(customerUpdate, customerId);
                if (serviceResult.MISACode == EnumServiceResult.Success)
                {
                    return StatusCode(200, serviceResult.Data);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnCustomer.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
        }

        /// <summary>
        /// Xóa 1 khách hàng trong db
        /// </summary>
        /// <param name="customerId">Id khách hàng muốn xóa</param>
        /// <returns>Số bản ghi khách hàng được xóa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(Guid customerId)
        {
            try
            {
                var result = _customerRepository.Delete(customerId);
                // trả về kết quả cho client
                if (result > 0)
                {
                    return StatusCode(200, result);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnCustomer.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
        }
        #endregion


    }


}
