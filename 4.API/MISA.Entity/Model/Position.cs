using MISA.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Model
{
    public class Position : Base
    {
        #region Propperty
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Mã vị trí
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id cha
        /// </summary>
        public string ParentId { get; set; }
        #endregion
    }
}
