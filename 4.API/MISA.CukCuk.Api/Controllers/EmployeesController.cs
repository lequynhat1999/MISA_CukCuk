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
using System.IO;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BaseEntityController<Employee>
    {
        #region DECLARE
        IEmployeeService _employeeService;
        IEmployeeRepository _employeeRepository;
        #endregion
        #region Constructor
        public EmployeesController(IEmployeeService employeeService, IEmployeeRepository employeeRepository) : base(employeeRepository, employeeService)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }
        #endregion

        #region Methods
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
                var employeesFilter = _employeeRepository.GetByPaging(pageIndex, pageSize, positionId, departmentId, keysearch);
                //if (employeesFilter.Count() > 0)
                //{
                //    return StatusCode(200, employeesFilter);
                //}
                //else
                //{
                //    var msg = new
                //    {
                //        userMsg = Properties.ResourceVnEmployee.User_ErrorMsg_NoContent,
                //    };
                //    return StatusCode(204, msg);
                //}
                return StatusCode(200, employeesFilter);
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

        [HttpPost("Import")]
        public IActionResult Import(IFormFile formFile)
        {
            try
            {
                var serviceResult = _employeeService.ImportEmployee(formFile);
                return Ok(serviceResult.Data);
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

        [HttpGet("Export")]
        public IActionResult ExportEmployee()
        {
            try
            {
                var serviceResult = _employeeService.ExportEmployee();
                if (serviceResult.Data != null)
                {
                    Stream stream = (Stream)serviceResult.Data;
                    stream.Position = 0;
                    string excelName = $"Employees.xlsx";
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
                else
                {
                    return Ok("Thuc hien khong thanh cong");
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

        #endregion
    }
}
