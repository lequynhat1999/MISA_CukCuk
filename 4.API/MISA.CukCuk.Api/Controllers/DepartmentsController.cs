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
    public class DepartmentsController : BaseEntityController<Department>
    {
        #region DECLARE
        IBaseRepository<Department> _baseRepository;
        IBaseService<Department> _baseService;
        #endregion

        #region Constructor
        public DepartmentsController(IBaseRepository<Department> baseRepository, IBaseService<Department> baseService) : base(baseRepository, baseService)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
        }
        #endregion
    }
}
