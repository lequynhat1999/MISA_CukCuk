using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Entity
{
    /// <summary>
    /// Enum để xác định trạng thái của việc validate
    /// </summary>
    public enum MISAEnum
    {
        /// <summary>
        /// trạng thái dữ liệu hợp lệ
        /// </summary>
        IsValid = 100,

        /// <summary>
        /// trạng thái dữ liệu không hợp lệ
        /// </summary>
        NotValid = 900,

        /// <summary>
        /// trạng thái thao tác thành công
        /// </summary>
        Success = 200,
    }
}
