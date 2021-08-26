using Microsoft.AspNetCore.Http;
using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        ServiceResult ExportEmployee();

        ServiceResult ImportEmployee(IFormFile file);
    }
}
