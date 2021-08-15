using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        IDepartmentService _departmentService;
        IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService)
        {
            _departmentRepository = departmentRepository;
            _departmentService = departmentService;
        }

        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về phòng ban trong db
        /// </summary>
        /// <returns>Danh sách phòng ban</returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet]
        public IActionResult GetDepartment()
        {
            try
            {
                // 4. trả kết quả về cho client
                var departments = _departmentRepository.Get();
                if (departments != null)
                {
                    return StatusCode(200, departments);
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
        /// lấy ra dữ liệu về 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban</param>
        /// <returns>Phòng ban tìm kiếm theo Id</returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet("{departmentId}")]
        public IActionResult GetDepartmentById(Guid departmentId)
        {
            try
            {
                // 4. trả kết quả về cho client
                var department = _departmentRepository.GetById(departmentId);
                if (department != null)
                {
                    return StatusCode(200, department);
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
        /// Thêm mới 1 phòng ban vào trong db
        /// </summary>
        /// <param name="department">dữ liệu phòng ban thêm mới</param>
        /// <returns></returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertDepartment(Department department)
        {
            try
            {
                // 4. trả kết quả về client
                var serviceResult = _departmentService.Add(department);
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
        /// Sửa thông tin 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">id phòng ban muốn sửa</param>
        /// <param name="department">dữ liệu phòng ban muốn sửa</param>
        /// <returns>Số bản ghi được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{departmentId}")]
        public IActionResult UpdateDepartment(Department department, Guid departmentId)
        {
            try
            {
                // 4. trả về cho client
                var serviceResult = _departmentService.Update(department, departmentId);
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
        /// Xóa 1 phòng ban trong db
        /// </summary>
        /// <param name="departmentId">Id phòng ban muốn xóa</param>
        /// <returns>Số bản ghi đã xóa trong db</returns>
        /// CreateBy: LQNhat(09/08/2021)
        [HttpDelete("{departmentId}")]
        public IActionResult DeleteDepartment(Guid departmentId)
        {
            try
            {
                // 4. trả kết quả về client
                var result = _departmentRepository.Delete(departmentId);
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
