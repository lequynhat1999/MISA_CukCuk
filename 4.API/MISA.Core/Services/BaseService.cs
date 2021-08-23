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
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : Base
    {
        #region DECLARE
        IBaseRepository<TEntity> _baseRepository;
        ServiceResult _serviceResult;
        // mảng chứa error
        List<string> messageArr = new List<string>();
        // check vaidate
        bool isValid = true;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { MISACode = MISAEnum.EnumServiceResult.Success };
        }
        #endregion

        #region Methods
        /// <summary>
        /// Xử lý nghiệp vụ việc thêm mới 1 đối tượng vào db
        /// </summary>
        /// <param name="entity">Đối tượng truyền vào</param>
        /// <returns>ServiceResult - lưu trạng thái kết quả sau khi xử lý nghiệp vụ và thao tác với db </returns>
        /// CreateBy: LQNHAT(18/08/2021)
        public virtual ServiceResult Add(TEntity entity)
        {
            // xử lý nghiệp vụ, validate data
            entity.EntityState = MISAEnum.EnumEntityState.Add;
            var isValid = Validate(entity);
            if (isValid == false)
            {
                return _serviceResult;
            }
            else
            {
                // thao tác với db
                _serviceResult.Data = _baseRepository.Add(entity);
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.Created;
                return _serviceResult;
            }
        }

        /// <summary>
        /// Xử lý nghiệp vụ việc sửa thông tin 1 đối tượng vào db
        /// </summary>
        /// <param name="entity">Đối tượng truyền vào</param>
        /// <param name="entityId">Id của đối tượng truyền vào</param>
        /// <returns>ServiceResult - lưu trạng thái kết quả sau khi xử lý nghiệp vụ và thao tác với db </returns>
        /// CreateBy: LQNHAT(18/08/2021)
        public virtual ServiceResult Update(TEntity entity, Guid entityId)
        {
            // xử lý nghiệp vụ
            entity.EntityState = MISAEnum.EnumEntityState.Update;
            var isValid = Validate(entity);
            if (isValid == false)
            {
                return _serviceResult;
            }
            else
            {
                // thao tác với db
                _serviceResult.Data = _baseRepository.Update(entity, entityId);
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.Success;
                return _serviceResult;
            }
        }

        /// <summary>
        /// Hàm xử lý nghiệp vụ, validate data
        /// </summary>
        /// <param name="entity">Đối tượng truyền vào</param>
        /// <returns>IsValid: True or False</returns>
        /// CreateBy: LQNHAT(18/08/2021)
        private bool Validate(TEntity entity)
        {
            // đọc các property
            var properties = entity.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);

                // lấy ra name của property
                var attrNames = prop.GetCustomAttributes(typeof(Name), true);
                var fieldName = string.Empty;
                if (attrNames.Length > 0)
                {
                    fieldName = (attrNames[0] as Name).FieldName;
                }

                // kiểm tra xem có attr cần phải validate k
                if (prop.IsDefined(typeof(Required), false))
                {
                    // check bắt buộc nhập
                    if (string.IsNullOrEmpty(Convert.ToString(propValue)) || propValue == null)
                    {
                        messageArr.Add(string.Format(Resources.ResourceVnEmployee.Error_Required, fieldName));
                        _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                        _serviceResult.Message = Resources.ResourceVnEmployee.Error_Validate;
                        isValid = false;
                    }
                    //isValid = IsRequired(propValue, fieldName);
                }

                if (prop.IsDefined(typeof(CheckExist), false))
                {
                    // Check trùng dữ liệu
                    var entityCheckExist = _baseRepository.GetByProperty(entity, prop);
                    if (entityCheckExist != null)
                    {
                        isValid = false;
                        messageArr.Add(string.Format(Resources.ResourceVnEmployee.Error_Exist, fieldName));
                        _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                        _serviceResult.Message = Resources.ResourceVnEmployee.Error_Validate;
                    }
                }

                if (prop.IsDefined(typeof(CheckEmail), false))
                {
                    // Check định dạng email
                    var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                    var isMatch = Regex.IsMatch((string)propValue, emailFormat, RegexOptions.IgnoreCase);
                    if (isMatch == false)
                    {
                        isValid = false;
                        messageArr.Add(string.Format(Resources.ResourceVnEmployee.Error_EmailFormat, fieldName));
                        _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                        _serviceResult.Message = Resources.ResourceVnEmployee.Error_Validate;
                    }
                }
            }
            _serviceResult.Data = messageArr;
            return isValid;
        }

        private bool IsRequired(object propValue, string fieldName)
        {
            if (string.IsNullOrEmpty(Convert.ToString(propValue)) || propValue == null)
            {
                messageArr.Add(string.Format(Resources.ResourceVnEmployee.Error_Required, fieldName));
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Message = Resources.ResourceVnEmployee.Error_Validate;
                return false;
            }
            else
            {
                return true;
            }
        }

        public ServiceResult DeleteEntites(string entitesId)
        {

            string[] arrId = entitesId.Split(",");
            List<Guid> idList = new List<Guid>();
            bool flag = true;
            for (int i = 0; i < arrId.Length; i++)
            {
                Guid temp = new Guid();
                if (Guid.TryParse(arrId[i], out temp))
                {
                    idList.Add(temp);
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                var result = _baseRepository.DeleteEntites(idList);
                if (!result)
                {
                    _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                    _serviceResult.Message = $"Id sai hoặc không tồn tại";
                }
            }
            else
            {
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                _serviceResult.Message = $"Id sai hoặc không tồn tại";
            }
            return _serviceResult;
        }
        #endregion
    }
}
