﻿using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.MISAAttribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        //IConfiguration _configuration;
        IDbConnection _dbConnection;
        public readonly string _connectionString;
        string _className;
        ServiceResult _serviceResult;

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LocalDatabase");
            _className = typeof(MISAEntity).Name;
            _serviceResult = new ServiceResult();
        }

        /// <summary>
        /// Hàm lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 28/08/2021
        public List<MISAEntity> GetAll()
        {      
            // 2.Khởi tạo đối tượng kết nối với database
            _dbConnection = new MySqlConnection(_connectionString);
            // 3.Lấy dữ liệu
            var sqlCommand = $"SELECT * FROM {_className}";
            var entities = _dbConnection.Query<MISAEntity>(sqlCommand);
            return entities.ToList();
        }

        /// <summary>
        /// Hàm lấy dữ liệu theo id
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 28/08/2021
        public MISAEntity GetById(Guid entityId)
        {
            
            // 2.Khởi tạo đối tượng kết nối với database
            _dbConnection = new MySqlConnection(_connectionString);
            // 3.Lấy dữ liệu
            var sqlCommand = $"SELECT * FROM {_className} WHERE {_className}Id = @EntityIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EntityIdParam", entityId);
            var entity = _dbConnection.QueryFirstOrDefault<MISAEntity>(sqlCommand, param: parameters);
            return entity;
        }

        /// <summary>
        /// Hàm thêm mới dữ liệu
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 28/08/2021
        public int Add(MISAEntity entity)
        {
          
            // 2.Khởi tạo đối tượng kết nối với database
             _dbConnection = new MySqlConnection(_connectionString);
            //khai báo dynamicParam:
            var dynamicParam = new DynamicParameters();
           
            // 3.Thêm dữ liệu vào database
            var columnsName = string.Empty;
            var columnsParam = string.Empty;

            //Đọc từng property của object:
            var properties = entity.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                // kiểm tra có thuộc tính nào là not map không
                var notMapProp = prop.GetCustomAttributes(typeof(MISANotMap),true);

                // các thuộc tính là not map sẽ ko dc thêm vào database
                if(notMapProp.Length == 0)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;
                    // Nếu  tên của prop là EmployeeId,CustomerId...

                    //Lấy value của prop:
                    var propValue = prop.GetValue(entity);
                    if (propName == $"{_className}Id" && prop.PropertyType == typeof(Guid))
                    {
                        //sinh id mới
                        propValue = Guid.NewGuid();
                    }

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                }            
            }
          
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

    
            var sqlCommand = $"INSERT INTO {_className} ({columnsName}) VALUES({columnsParam}) ";
            var rowEffects = _dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }

        /// <summary>
        /// Hàm sửa dữ liệu
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 28/08/2021
        public int Edit(MISAEntity entity, Guid entityId)
        {
            
            // 2.Khởi tạo đối tượng kết nối với database
            _dbConnection = new MySqlConnection(_connectionString);
            //khai báo dynamicParam:
            var dynamicParam = new DynamicParameters();

            // 3.Thêm dữ liệu vào database
            var columnsName = string.Empty;

            //Đọc từng property của object:
            var properties = entity.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(entity);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //thêm param tương ứng với mỗi property của đối tượng
                dynamicParam.Add($"@{propName}", propValue);

                columnsName += $"{propName} = @{propName},";

            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);

            var sqlCommand = $"UPDATE {_className} SET {columnsName} WHERE {_className}Id = @EntityIdParam ";

            dynamicParam.Add("@EntityIdParam", entityId);
            var rowEffects = _dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }


        /// <summary>
        /// Hàm kiểm tra trùng mã
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 28/08/2021
        public bool checkedCodeExist(string entityCode, Guid entityId)
        {
            // 2.Khởi tạo đối tượng kết nối với database
            _dbConnection = new MySqlConnection(_connectionString);

            var sqlCommand = $"SELECT * FROM {_className} WHERE {_className}Code = @EntityCodeParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EntityCodeParam", entityCode);
            var entityCheck = _dbConnection.QueryFirstOrDefault<MISAEntity>(sqlCommand, param: parameters);

            var propValue = "";
            if(entityCheck != null)
            {
                propValue = entityCheck.GetType().GetProperty($"{_className}Id").GetValue(entityCheck).ToString();
            }
    

            if (entityId == Guid.Empty)
            {
                if(entityCheck != null)
                {
                    return false;
                }
            }
            else
            {
                if (entityCheck != null && entityId.ToString() != propValue)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Hàm xóa dữ liệu
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 28/08/2021
        public int Delete(Guid entityId)
        {
            // 2.Khởi tạo đối tượng kết nối với database
            _dbConnection = new MySqlConnection(_connectionString);

            // 3.Lấy dữ liệu
            var sqlCommand = $"DELETE FROM {_className} WHERE {_className}Id = @EntityIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EntityIdParam", entityId);
            var rowEffects = _dbConnection.Execute(sqlCommand, param: parameters);
            return rowEffects;
        }

        public ServiceResult MultiDelete(List<Guid> ids)
        {
            // 2.Khởi tạo đối tượng kết nối với database
            _dbConnection = new MySqlConnection(_connectionString);

            _dbConnection.Open();
            var transaction = _dbConnection.BeginTransaction();
            var rowEffects = 0;
            var sqlCommand = $"DELETE FROM {_className} WHERE {_className}Id = @EntityIdParam";
            // 3.Lấy dữ liệu
            foreach (var id in ids)
            {            
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EntityIdParam", id);
                rowEffects += _dbConnection.Execute(sqlCommand, param: parameters,transaction: transaction);    
            }
            if(rowEffects == ids.Count())
            {
                transaction.Commit();
                _serviceResult.Data = rowEffects;
            }
            else
            {
                _serviceResult.Message = MISA.Core.Properties.ResourceVN.Cannot_Delete_All;
                _serviceResult.isValid = false;
                transaction.Rollback();              
            }
                
            return _serviceResult;
        }
    }
}
