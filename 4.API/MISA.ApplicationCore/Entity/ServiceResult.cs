using MISA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entity
{
    public class ServiceResult
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public MISAEnum MISAErrorCode { get; set; }

    }
}
