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
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Base
    {
        #region DECLARE
        IConfiguration _configuration;
        string _connectionString = string.Empty;
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
            return result;
        }

        public int Delete(Guid entityId)
        {
            // xóa dữ liệu
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@entityIdParam", entityId);
            var sqlQuery = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @entityIdParam";
            var result = _dbConnection.Execute(sqlQuery, param: parameters);
            return result;
        }

        public IEnumerable<TEntity> Get()
        {
            // lấy dữ liệu
            var sqlQuery = $"SELECT * FROM {_tableName}";
            var entities = _dbConnection.Query<TEntity>(sqlQuery);
            return entities;
        }

        public TEntity GetById(Guid entityId)
        {
            // 3. lấy dữ liệu
            var sqlQuery = $"SELECT * FROM {_tableName} WHERE {_tableName}Id = @entityId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@entityId", entityId);
            var entity = _dbConnection.QueryFirstOrDefault<TEntity>(sqlQuery, param: parameters);
            return entity;
        }

        public int Update(TEntity entity, Guid entityId)
        {
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
            //DynamicParameters dynamicParameters = new DynamicParameters();
            //dynamicParameters.Add("@entityId", entityId);
            var sqlQuery = $"UPDATE {_tableName} SET {columnsName} WHERE {_tableName}Id = '{entityId}'";
            var result = _dbConnection.Execute(sqlQuery, param: param);
            return result;
        }

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
        #endregion
    }
}
