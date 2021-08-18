using MISA.Core.MISAEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{

    #region Attribute
    /// <summary>
    /// Cờ check việc bắt buộc nhập
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Required : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CheckExist : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CheckEmail : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotMap : Attribute
    {

    }
    #endregion
    public class Base
    {
        #region Property
        [NotMap]
        public EnumEntityState EntityState { get; set; } = EnumEntityState.Add;

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Tạo bởi
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Được chỉnh sửa bởi
        /// </summary>
        public string ModifiedBy { get; set; }
        #endregion
    }
}
