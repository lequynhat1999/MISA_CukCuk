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
    public class PositionsController : ControllerBase
    {
        IPositionService _postionService;
        IPositionRepository _positionRepository;
        public PositionsController(IPositionRepository positionRepository, IPositionService positionService)
        {
            _positionRepository = positionRepository;
            _postionService = positionService;
        }

        /// <summary>
        /// Lấy ra toàn bộ dữ liệu về vị trí trong db
        /// </summary>
        /// <returns>Danh sách vị trí</returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet]
        public IActionResult GetPosition()
        {
            try
            {
                // 4. trả kết quả về cho client
                var positions = _positionRepository.Get();
                if (positions != null)
                {
                    return StatusCode(200, positions);
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
        /// lấy ra dữ liệu về 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí</param>
        /// <returns>Vị trí tìm theo Id</returns>
        /// CreateBy: LQNhat(9/8/2021)
        [HttpGet("{positionId}")]
        public IActionResult GetPositionById(Guid positionId)
        {
            try
            {
                // 4. trả kết quả về cho client
                var position = _positionRepository.GetById(positionId);
                if (position != null)
                {
                    return StatusCode(200, position);
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
        /// Thêm mới 1 vị trí vào trong db
        /// </summary>
        /// <param name="position">dữ liệu vị trí thêm mới</param>
        /// <returns>Số bản ghi được thêm trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPost]
        public IActionResult InsertPosition(Position position)
        {
            try
            {
                // 4. trả kết quả về client
                var serviceResult = _postionService.Add(position);
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
        /// Sửa thông tin 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">id vị trí muốn sửa</param>
        /// <param name="position">dữ liệu vị trí muốn sửa</param>
        /// <returns>Số bản ghi được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        [HttpPut("{positionId}")]
        public IActionResult UpdatePosition(Position position, Guid positionId)
        {
            try
            {
                // 4. trả về cho client
                var serviceResult = _postionService.Update(position, positionId);
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
        /// Xóa 1 vị trí trong db
        /// </summary>
        /// <param name="positionId">Id vị trí muốn xóa</param>
        /// <returns>Số bản ghi vừa xóa trong db</returns>
        /// CreateBy: LQNhat(09/08/2021)
        [HttpDelete("{positionId}")]
        public IActionResult DeletePosition(Guid positionId)
        {
            try
            {
                // 4. trả kết quả về client
                var result = _positionRepository.Delete(positionId);
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
