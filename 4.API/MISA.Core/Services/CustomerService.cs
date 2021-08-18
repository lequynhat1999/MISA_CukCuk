using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region DECLARE
        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository) : base(customerRepository)
        {
            _serviceResult = new ServiceResult();
            _customerRepository = customerRepository;
        }
        #endregion
    }
}
