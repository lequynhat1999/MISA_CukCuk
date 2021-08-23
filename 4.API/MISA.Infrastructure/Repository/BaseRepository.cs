using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : Base
    {
        #region DECLARE
        IConfiguration _configuration;
        readonly string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        string _tableName;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Thêm mới 1 đối tượng vào trong db
        /// </summary>
        /// <param name="entity">object muốn thêm</param>
        /// <returns>Số bản ghi được thêm vào db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Add(TEntity entity)
        {
            // chuỗi chứa tên cột
            var columnsName = string.Empty;

            // chuỗi chứa param
            var columnsParam = string.Empty;

            var properties = entity.GetType().GetProperties();

            var param = new DynamicParameters();

            foreach (var prop in properties)
            {
                // check attr NotMap
                var propertyAttrNotMap = prop.GetCustomAttributes(typeof(NotMap), true);
                if (propertyAttrNotMap.Length == 0)
                {
                    var propName = prop.Name;
                    var propValue = prop.GetValue(entity);
                    // sinh mã mới
                    if (propName == $"{_tableName}Id" && prop.PropertyType == typeof(Guid))
                    {
                        propValue = Guid.NewGuid();
                    }
                    // ngày tạo
                    if (propName == "CreatedDate")
                    {
                        propValue = DateTime.UtcNow;
                    }

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                    param.Add($"@{propName}", propValue);
                }
            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
            // thêm dữ liệu vào db
            var sqlQuery = $"INSERT INTO {_tableName}({columnsName}) VALUES({columnsParam}) ";
            var result = _dbConnection.Execute(sqlQuery, param: param);
            //transaction.Commit();

            return result;
        }

        /// <summary>
        /// Xóa 1 đối tượng trong db
        /// </summary>
        /// <param name="entityId">Id đối tượng muốn xóa</param>
        /// <returns>Số bản ghi đối tượng được xóa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Delete(Guid entityId)
        {
            // xóa dữ liệu
            var entityCurrent = GetById(entityId);
            if (entityCurrent != null)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@entityIdParam", entityId);
                var sqlQuery = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @entityIdParam";
                var result = _dbConnection.Execute(sqlQuery, param: parameters);
                //transaction.Commit();
                return result;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// Lấy ra tất cả thông tin của đối tượng trong database
        /// </summary>
        /// <returns>Danh sách đối tượng trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public virtual IEnumerable<TEntity> Get()
        {
            try
            {
                // lấy dữ liệu
                var sqlQuery = $"SELECT * FROM {_tableName}";
                var entities = _dbConnection.Query<TEntity>(sqlQuery);
                return entities;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Lấy ra thông tin của 1 đối tượng theo Id
        /// </summary>
        /// <param name="entityId">Id của đối tượng muốn lấy</param>
        /// <returns>Thông tin đối tượng muốn tìm theo Id</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public TEntity GetById(Guid entityId)
        {
            try
            {
                // 3. lấy dữ liệu
                var sqlQuery = $"SELECT * FROM {_tableName} WHERE {_tableName}Id = @entityId";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@entityId", entityId);
                var entity = _dbConnection.QueryFirstOrDefault<TEntity>(sqlQuery, param: parameters);
                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Cập nhật thông tin đối tượng trong db
        /// </summary>
        /// <param name="entityId">Id đối tượng muốn cập nhật</param>
        /// <param name="entity">Dữ liệu muốn cập nhật</param>
        /// <returns>Số bản ghi đối tượng được sửa trong db</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public int Update(TEntity entity, Guid entityId)
        {
            _dbConnection.Open();
            var entityCurrent = GetById(entityId);
            if (entityCurrent != null)
            {
                var transaction = _dbConnection.BeginTransaction();
                var columnsName = string.Empty;
                var param = new DynamicParameters();
                var properties = entity.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    var propertyAttrNotMap = prop.GetCustomAttributes(typeof(NotMap), true);
                    if (propertyAttrNotMap.Length == 0)
                    {
                        var propName = prop.Name;
                        var propValue = prop.GetValue(entity);
                        //ngày chỉnh sửa
                        if (propName == "ModifiedDate")
                        {
                            propValue = DateTime.UtcNow;
                        }
                        columnsName += $"{propName} = @{propName},";
                        param.Add($"@{propName}", propValue);
                    }
                }

                // cắt dấu phẩy cuối chuỗi
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                // sửa dữ liệu
                var sqlQuery = $"UPDATE {_tableName} SET {columnsName} WHERE {_tableName}Id = '{entityId}'";
                var result = _dbConnection.Execute(sqlQuery, transaction: transaction, param: param);
                transaction.Commit();
                return result;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Lấy ra đối tượng theo Property
        /// </summary>
        /// <param name="entity">Đối tượng muôn lấy</param>
        /// <param name="property">Property muốn tìm kiếm</param>
        /// <returns>Đối tượng được lấy ra theo điều kiện Property</returns>
        /// CreateBy:LQNhat(09/08/2021)
        public TEntity GetByProperty(TEntity entity, PropertyInfo property)
        {
            var propName = property.Name;
            var propValue = property.GetValue(entity);
            var keyValue = entity.GetType().GetProperty($"{_tableName}Id").GetValue(entity).ToString();
            var sqlQuery = string.Empty;
            if (entity.EntityState == Core.MISAEnum.EnumEntityState.Add)
            {
                sqlQuery = $"SELECT * FROM {_tableName} WHERE {propName} = '{propValue}'";
            }
            else if (entity.EntityState == Core.MISAEnum.EnumEntityState.Update)
            {
                sqlQuery = $"SELECT * FROM {_tableName} WHERE {propName} = '{propValue}' AND {_tableName}Id <> '{keyValue}'";
            }
            var entityGetByProperty = _dbConnection.QueryFirstOrDefault<TEntity>(sqlQuery);
            return entityGetByProperty;
        }


        public bool DeleteEntites(List<Guid> entitesId)
        {
            bool flag = true;
            try
            {
                _dbConnection.Open();
                using (var transaction = _dbConnection.BeginTransaction())
                {
                    foreach (var item in entitesId)
                    {
                        string sqlQuery = $"DELETE FROM {_tableName} WHERE {_tableName}Id = '{item}'";
                        var result = _dbConnection.Execute(sqlQuery, transaction: transaction);
                        if (result == 0)
                        {
                            flag = false;
                            transaction.Rollback();
                            break;
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        /// <summary>
        /// Hàm ngắt kết nối
        /// </summary>
        public void Dispose()
        {
            _dbConnection.Dispose();
        }


        #endregion
    }
}
