using MISA.Core.MISAEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class ServiceResult
    {
        /// <summary>
        /// Kết quả nhận vào
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Thông điệp nhận vào
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Check sự hợp lệ
        /// </summary>
        public EnumServiceResult MISACode { get; set; }

    }
}
