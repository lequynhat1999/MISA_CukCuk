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
    public class CustomersController : BaseEntityController<Customer>
    {
        #region DECLARE
        IBaseRepository<Customer> _baseRepository;
        IBaseService<Customer> _customerService;
        #endregion

        #region Constructor
        public CustomersController(IBaseRepository<Customer> baseRepository, IBaseService<Customer> customerService) : base(baseRepository, customerService)
        {
            _baseRepository = baseRepository;
            _customerService = customerService;
        }
        #endregion
    }


}
