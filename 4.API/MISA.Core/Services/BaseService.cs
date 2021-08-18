using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { MISACode = MISAEnum.EnumServiceResult.Success };
        }
        #endregion

        #region Methods
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

        private bool Validate(TEntity entity)
        {
            var isValid = true;
            var messageArr = new List<string>();
            // đọc các property
            var properties = entity.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);
                var displayName = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                // kiểm tra xem có attr cần phải validate k
                if (prop.IsDefined(typeof(Required), false))
                {
                    // check bắt buộc nhập
                    if (string.IsNullOrEmpty(Convert.ToString(propValue)) || propValue == null)
                    {
                        isValid = false;
                        messageArr.Add($"Thông tin {propName} không được phép để trống");
                        _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                        _serviceResult.Message = "Dữ liệu nhập vào không hợp lệ";
                    }
                }
                if (prop.IsDefined(typeof(CheckExist), false))
                {
                    // Check trùng dữ liệu
                    var entityCheckExist = _baseRepository.GetByProperty(entity, prop);
                    if (entityCheckExist != null)
                    {
                        isValid = false;
                        messageArr.Add($"Thông tin {propName} đã tồn tại");
                        _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                        _serviceResult.Message = "Dữ liệu nhập vào không hợp lệ";
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
                        messageArr.Add($"Thông tin {propName} sai định dạng");
                        _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                        _serviceResult.Message = "Dữ liệu nhập vào không hợp lệ";
                    }
                }
            }
            _serviceResult.Data = messageArr;
            return isValid;
        }
        #endregion
    }
}
