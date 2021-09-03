using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISA.Core.Entities;
using System.Data;
using MySqlConnector;
using Dapper;
using System.Text.RegularExpressions;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MISA.Core.Enumeration;
using System.Threading;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MISA.Core.CommonFunction;
using System.Drawing;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BaseEntityController<Employee>
    {
        #region DECLARE
        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        IBaseRepository<Employee> _baseRepository;
        IBaseService<Employee> _baseService;

        #endregion

        #region Constructor
        public EmployeesController(IBaseService<Employee> baseService, IBaseRepository<Employee> baseRepository,
            IEmployeeRepository employeeRepository, IEmployeeService employeeService) : base(baseService, baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;

        }
        #endregion

        #region Methods
        /// <summary>
        /// Phân trang nhân viên
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchContent"></param>
        /// <param name="positionId"></param>
        /// <param name="departmentId"></param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: Ngọc 28/08/2021
        [HttpGet("Paging")]
        public IActionResult Pagination(int? pageSize, int? pageNumber, string searchContent)
        {
            try
            {
                var pagingResult = _employeeRepository.Pagination(pageSize, pageNumber, searchContent);
                // Trả về cho client
                if (pagingResult.TotalPageNumber == 0)
                {
                    return StatusCode(204);
                }
                return Ok(pagingResult);
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }

        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên</returns>
        /// Created By: Ngọc 28/08/2021
        [HttpGet("NewEmployeeCode")]
        public IActionResult getNewCode()
        {
            try
            {
                var newEmployeeCode = _employeeRepository.GetNewCode();
                // Trả về cho client
                return Ok(newEmployeeCode);
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }

        public override IActionResult InsertEntity(Employee entity)
        {
            try
            {
                var serviceResult = _employeeService.Add(entity);

                // Trả về cho client
                if (serviceResult.isValid == true)
                {
                    return StatusCode(201, serviceResult.Data);
                }
                else
                {
                    var errorObj = new
                    {
                        userMsg = serviceResult.Message,
                        errorCode = "misa-001",
                        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                        traceId = ""
                    };
                    return StatusCode(400, errorObj);
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }

        public override IActionResult UpdateEntity(Guid entityId, Employee entity)
        {
            try
            {
                var serviceResult = _employeeService.Edit(entity, entityId);

                // Trả về cho client
                if (serviceResult.isValid == true)
                {
                    return StatusCode(200, serviceResult.Data);
                }
                else
                {
                    var errorObj = new
                    {
                        userMsg = serviceResult.Message,
                        errorCode = "misa-001",
                        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                        traceId = ""
                    };
                    return StatusCode(400, errorObj);
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }

        /// <summary>
        /// Xuất khẩu dữ liệu vào excel
        /// </summary>
        /// <returns></returns>
        /// Created By : Ngọc 02/09/2021
        [HttpGet("export")]
        public IActionResult Export()
        {
            try
            {
                // gọi lên service để lấy về list customer
                var fileStream = (MemoryStream)_employeeService.Export().Data;
                
                string excelName = $"EmployeeList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                //return File(stream, "application/octet-stream", excelName);  
                return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }

        }
        #endregion
    }


}


