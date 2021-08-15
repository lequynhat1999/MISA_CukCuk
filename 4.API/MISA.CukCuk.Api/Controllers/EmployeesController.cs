using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySqlConnector;
using Dapper;
using System.Text.RegularExpressions;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Services;
using MISA.Core.Interfaces.Repository;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        IEmployeeService _employeeService;
        IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Lấy ra dữ liệu của toàn bộ nhân viên trong db
        /// </summary>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                var employees = _employeeRepository.Get();
                // 4. Trả về Client
                if (employees.Count() > 0)
                {
                    return StatusCode(200, employees);
                }
                else
                {
                    var msg = new
                    {
                        userMsg = Properties.ResourceVnEmployee.User_ErrorMsg_NoContent,
                    };
                    return StatusCode(204, msg);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnEmployee.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
        }

        /// <summary>
        /// Lấy ra dữ liệu 1 nhân viên theo Id
        /// </summary>
        /// <param name="employeeId">Id nhân viên muốn lấy ra</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
        {
            try
            {
                var employee = _employeeRepository.GetById(employeeId);
                // 4. trả về client
                if (employee != null)
                {
                    return StatusCode(200, employee);
                }
                else
                {
                    var msg = new
                    {
                        userMsg = Properties.ResourceVnEmployee.User_ErrorMsg_NoContent,
                    };
                    return StatusCode(204, msg);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnEmployee.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
        }

        /// <summary>
        /// Tự sinh mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới dạng string</returns>
        /// CreateBy: LQNHAT(13/08/2021)
        [HttpGet("NewEmployeeCode")]
        public IActionResult AutoNewEmployeeCode()
        {
            try
            {
                // 4. trả về client
                var newEmployeeCode = _employeeRepository.NewCode();
                return StatusCode(200, newEmployeeCode);
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnEmployee.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }

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
        [HttpGet("filter")]
        public IActionResult GetEmployeesPaging(int pageIndex, int pageSize, string positionId, string departmentId, string keysearch)
        {
            try
            {
                // 4. trả về cho client
                var employees = _employeeRepository.GetByPaging(pageIndex, pageSize, positionId, departmentId, keysearch);
                if (employees.Count() > 0)
                {
                    return StatusCode(200, employees);
                }
                else
                {
                    var msg = new
                    {
                        userMsg = Properties.ResourceVnEmployee.User_ErrorMsg_NoContent,
                    };
                    return StatusCode(204, msg);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourceVnEmployee.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }
        }

        /// <summary>
        /// Thêm 1 nhân viên vào db
        /// </summary>
        /// <param name="employee">dữ liệu về nhân viên muốn thêm</param>
        /// <returns>Số bản ghi được thêm vào trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertEmployee(Employee employee)
        {
            try
            {
                var serviceResult = _employeeService.Add(employee);
                // trả kết quả về cho client
                if (serviceResult.MISACode == Core.MISAEnum.EnumServiceResult.Created)
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
                    userMsg = Properties.ResourceVnEmployee.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }

        }

        /// <summary>
        /// Sửa thông tin 1 nhân viên trong db
        /// </summary>
        /// <param name="employeeId">Id của nhân viên muốn sửa</param>
        /// <param name="employee">Dữ liệu nhân viên muốn sửa</param>
        /// <returns>Số bản ghi được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Employee employee, Guid employeeId)
        {
            try
            {
                var serviceResult = _employeeService.Update(employee, employeeId);
                // 4. trả về cho client
                if (serviceResult.MISACode == Core.MISAEnum.EnumServiceResult.Success)
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
                    userMsg = Properties.ResourceVnEmployee.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }


        }

        /// <summary>
        /// Xóa 1 nhân viên trong db
        /// </summary>
        /// <param name="employeeId">Id của nhân viên</param>
        /// <returns>Số dòng được xóa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(Guid employeeId)
        {
            try
            {
                // 4. trả kết quả về client
                var result = _employeeRepository.Delete(employeeId);
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
                    userMsg = Properties.ResourceVnEmployee.Exception_ErrorMsg,
                };
                return StatusCode(500, msg);
            }

        }
    }
}
