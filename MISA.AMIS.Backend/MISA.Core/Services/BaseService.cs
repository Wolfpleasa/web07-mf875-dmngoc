using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MISA.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region DECLARE
        IBaseRepository<MISAEntity> _baseRepository;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _serviceResult = new ServiceResult();
            _baseRepository = baseRepository;         
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thực hiện thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy : Ngọc 28/08/2021
        public virtual ServiceResult Add(MISAEntity entity)
        {
            var className = typeof(MISAEntity).Name;
            var entityCode = entity.GetType().GetProperty($"{className}Code").GetValue(entity).ToString();
            //validate dữ liệu và xử lí nghiệp vụ
            var isValid = true;
            //validate các trường bắt buộc
            isValid = validateRequired(entity);
            if (isValid)
            {
                //validate Email
                isValid = validateEmail(entity);
            }

            if (isValid)
            {
                //validate các trường chỉ chứa số
                isValid = validateIsNumber(entity);
            }

            if (isValid)
            {
                //validate các trường ngày tháng
                isValid = validateFieldDate(entity);
            }

            if (isValid)
            {
                //check trùng mã
                isValid = checkedCodeExist(entityCode, Guid.Empty);
            }

            if (isValid)
            {
                //Thực hiện thêm mới
                _serviceResult.Data = _baseRepository.Add(entity);           
            }
            return _serviceResult;

        }

        /// <summary>
        /// Hàm thực hiện sửa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy : Ngọc 28/08/2021
        public virtual ServiceResult Edit(MISAEntity entity, Guid entityId)
        {
            //validate dữ liệu và xử lí nghiệp vụ
            var className = typeof(MISAEntity).Name;
            var entityCode = entity.GetType().GetProperty($"{className}Code").GetValue(entity).ToString();
            //validate dữ liệu và xử lí nghiệp vụ
            var isValid = true;

            //validate các trường bắt buộc
            isValid = validateRequired(entity);
            if (isValid)
            {   
                //validate Email
                isValid = validateEmail(entity);
            }

            if (isValid)
            {   
                //validate các trường chỉ chứa số
                isValid = validateIsNumber(entity);
            }

            if (isValid)
            {
                //validate các trường ngày tháng
                isValid = validateFieldDate(entity);
            }

            if (isValid)
            {   
                //validate trùng mã
                isValid = checkedCodeExist(entityCode, entityId);
            }

            if (isValid)
            {
                //Thực hiện sửa
                _serviceResult.Data = _baseRepository.Edit(entity, entityId);
            }

            return _serviceResult;      
        }
        #endregion

        #region Validate

        /// <summary>
        /// Hàm kiểm tra nhập các trường bắt buộc
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true/false</returns>
        /// CreatedBy : Ngọc 30/08/2021
        public bool validateRequired(MISAEntity entity)
        {
            var isValid = true;
            var properties = typeof(MISAEntity).GetProperties();
            foreach(var prop in properties)
            {
                var requiredProp = prop.GetCustomAttributes(typeof(MISARequired), true);
              
                if(requiredProp.Length > 0)
                {
                    var propName = (requiredProp[0] as MISARequired).Name;
                    var propValue = prop.GetValue(entity);
                    if (propValue == null || propValue == "")
                    {
                        isValid = false;
                        _serviceResult.Message = string.Format(Properties.ResourceVN.Empty_Field, propName);
                        _serviceResult.isValid = false;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Hàm kiểm tra định dạng Email
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true/false</returns>
        /// CreatedBy : Ngọc 30/08/2021
        public bool validateEmail(MISAEntity entity)
        {
            var isValid = true;
            var properties = typeof(MISAEntity).GetProperties();
            foreach (var prop in properties)
            {
                var emailProp = prop.GetCustomAttributes(typeof(MISAEmail), true);
                if (emailProp.Length > 0)
                {
                    // nếu email là null thì return true
                  
                    if (prop.GetValue(entity) == null || prop.GetValue(entity) == "") break;
                    var propValue = prop.GetValue(entity).ToString();
                    var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                    var isMatch = Regex.IsMatch(propValue, emailFormat, RegexOptions.IgnoreCase);
                    if (!isMatch)
                    {
                        isValid = false;
                        _serviceResult.Message = Properties.ResourceVN.Error_Email;
                        _serviceResult.isValid = false;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Hàm kiểm tra các trường chỉ chứa chữ số
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true/false</returns>
        /// CreatedBy : Ngọc 30/08/2021
        public bool validateIsNumber(MISAEntity entity)
        {
            var isValid = true;
            var properties = typeof(MISAEntity).GetProperties();
            foreach (var prop in properties)
            {
                var NumProp = prop.GetCustomAttributes(typeof(MISAIsNumber), true);
                if (NumProp.Length > 0)
                {

                    if (prop.GetValue(entity) == null || prop.GetValue(entity) == "") break;
                    var propName = (NumProp[0] as MISAIsNumber).Name.ToString();
                    var propValue = prop.GetValue(entity);
                    var NumFormat = @"^([0-9]\d*)$";
                    var isMatch = Regex.IsMatch(propValue.ToString(), NumFormat, RegexOptions.IgnoreCase);
                    if (!isMatch)
                    {
                        isValid = false;
                        _serviceResult.Message = string.Format(Properties.ResourceVN.Contain_Numbers_Only, propName);
                        _serviceResult.isValid = false;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Hàm kiểm tra các trường ngày tháng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true/false</returns>
        /// CreatedBy : Ngọc 30/08/2021
        public bool validateFieldDate(MISAEntity entity)
        {
            var isValid = true;
            var properties = typeof(MISAEntity).GetProperties();
            foreach (var prop in properties)
            {
                var NumProp = prop.GetCustomAttributes(typeof(MISADate), true);
                if (NumProp.Length > 0)
                {

                    if (prop.GetValue(entity) == null || prop.GetValue(entity) == "") break;
                    var propName = (NumProp[0] as MISADate).Name.ToString();
                    var propValue = prop.GetValue(entity);
                    var DateFormat = @"(19[0-9]{2}|20[0-1][0-9]|2020[-](0[1-9]|1[0-2])[-]([0][1-9]|[1-2][0-9]|3[0-1]))|(2021[-]((0[1-8])[-]([0][1-9]|[1-2][0-9]|3[0-1])|09[-]0[1-3]))$";
                    var isMatch = Regex.IsMatch(propValue.ToString(), DateFormat, RegexOptions.IgnoreCase);
                    if (!isMatch)
                    {
                        isValid = false;
                        _serviceResult.Message = string.Format(Properties.ResourceVN.Wrong_Date, propName);
                        _serviceResult.isValid = false;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Hàm kiểm tra trùng mã 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true/false</returns>
        /// CreatedBy : Ngọc 30/08/2021
        public virtual bool checkedCodeExist(string entityCode, Guid entityId)
        {
            var isValid = _baseRepository.checkedCodeExist(entityCode, entityId);
            if (!isValid)
            {
                _serviceResult.Message = Properties.ResourceVN.Duplicate_Code;
                _serviceResult.isValid = false;
            }
            return isValid;
        }

        #endregion
    }
}
