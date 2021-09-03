using MISA.Core.Entities;
using MISA.Core.Enumeration;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;
        IBaseRepository<Employee> _baseRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IBaseRepository<Employee> baseRepository) : base(baseRepository)
        {
            _serviceResult = new ServiceResult();
            _employeeRepository = employeeRepository;
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// Hàm ghi đè thực hiện thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy : Ngọc 29/08/2021
        public override ServiceResult Add(Employee entity)
        {
            try
            {
                //validate định dạng mã nv
                var propValue = entity.GetType().GetProperty("EmployeeCode").GetValue(entity).ToString().Trim();

                if (string.IsNullOrEmpty(propValue))
                {
                    _serviceResult.Message = Properties.ResourceVN.Empty_EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }
                                             
                var employeeCodeFormat = @"^(NV-\d+)$"; 
                var isMatch = Regex.IsMatch(propValue, employeeCodeFormat, RegexOptions.IgnoreCase);
                if (!isMatch)
                {
                    _serviceResult.Message = Properties.ResourceVN.EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }
                    
                return base.Add(entity);
            }
            catch(Exception)
            {
                _serviceResult.isValid = false;
                _serviceResult.Message = Properties.ResourceVN.Error_Message_UserVN;
                return _serviceResult;
            }               
        }


        /// <summary>
        /// Hàm ghi đè thực hiện sửa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy : Ngọc 29/08/2021
        public override ServiceResult Edit(Employee entity, Guid entityId)
        {
            try
            {
                //validate định dạng mã nv
                var propValue = entity.GetType().GetProperty("EmployeeCode").GetValue(entity).ToString().Trim();

                if (string.IsNullOrEmpty(propValue))
                {
                    _serviceResult.Message = Properties.ResourceVN.Empty_EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }

                var employeeCodeFormat = @"^(NV-\d+)$";
                var isMatch = Regex.IsMatch(propValue, employeeCodeFormat, RegexOptions.IgnoreCase);

                if (!isMatch)
                {
                    _serviceResult.Message = Properties.ResourceVN.EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }

                return base.Edit(entity, entityId);
            }
            catch (Exception)
            {
                _serviceResult.isValid = false;
                _serviceResult.Message = Properties.ResourceVN.Error_Message_UserVN;
                return _serviceResult;
            }

        }

        public  ServiceResult Export()
        {
            try
            {           
                // gọi lên service để lấy về list customer
                var employees = _baseRepository.GetAll();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    //Đặt tên người tạo file
                    package.Workbook.Properties.Author = "M. Ngọc";
                    //Đặt tiêu đề cho file
                    package.Workbook.Properties.Title = "Danh sách nhân viên";
                    // tạo sheet 
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    // tạo fontsize và fontfamily cho sheet
                    workSheet.Cells.Style.Font.Size = 11;
                    workSheet.Cells.Style.Font.Name = "Times New Roman";

                    // danh sách các tên cột
                    string[] arrColumnHeader = {"STT", "Mã nhân viên","Tên nhân viên", "Giới tính", "Ngày sinh",
                    "Chức danh", "Tên đơn vị", "Số tài khoản", "Tên ngân hàng"};
                    // Gán row header
                    for (var i = 0; i < arrColumnHeader.Length; i++)
                    {
                        workSheet.Cells[3, i + 1].Value = arrColumnHeader[i];
                    }
                    //// chỉnh style cho bảng
                    //workSheet.Row(1).Style.Font.Bold = true;
                    //workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                    //workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // chỉnh style cho hàng 1
                    workSheet.Cells[1, 1].Value = "DANH SÁCH NHÂN VIÊN";
                    workSheet.Cells["A1:I1"].Merge = true;
                    workSheet.Cells[1, 1].Style.Font.Size = 16;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Row(1).Style.Font.Name = "Aria";
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    workSheet.Cells["A2:I2"].Merge = true;

                    //Chỉnh style cho header
                    workSheet.Cells["A3:I3"].Style.Font.Bold = true;
                    workSheet.Cells["A3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
                    workSheet.Cells["A3:I3"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    workSheet.Cells["A3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //Chỉnh độ rộng các cột
                    for (int i = 0; i < 9; i++)
                    {
                        workSheet.Column(i + 1).Width = 15;
                    }

                    // chỉnh style cho cột
                    workSheet.Cells[$"A3:A{3 + employees.Count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    workSheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Gán data list vào sheet
                    var rowIndex = 4;
                    for (int i = 0; i < employees.Count(); i++)
                    {
                        workSheet.Cells[rowIndex, 1].Value = i + 1;
                        workSheet.Cells[rowIndex, 2].Value = employees[i].EmployeeCode;
                        workSheet.Cells[rowIndex, 3].Value = employees[i].FullName;
                        workSheet.Cells[rowIndex, 4].Value = Enumer.getGenderName(employees[i].Gender);
                        workSheet.Cells[rowIndex, 5].Value = String.Format("{0:dd/MM/yyyy}", employees[i].DateOfBirth);
                        workSheet.Cells[rowIndex, 6].Value = employees[i].PositionName;
                        workSheet.Cells[rowIndex, 7].Value = _employeeRepository.getDepartmentName(employees[i]);
                        workSheet.Cells[rowIndex, 8].Value = employees[i].BankAccount;
                        workSheet.Cells[rowIndex, 9].Value = employees[i].BankName;

                        for (int j = 0; j < 9; j++)
                        {
                            if (workSheet.Cells[rowIndex, j + 1].Value != null)
                            {
                                workSheet.Column(j + 1).Width = Math.Max(workSheet.Column(j + 1).Width, workSheet.Cells[rowIndex, j + 1].Value.ToString().Length + 5);
                            }
                        }
                        rowIndex++;
                    }
                    workSheet.Cells[$"A3:I{rowIndex - 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[$"A3:I{rowIndex - 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[$"A3:I{rowIndex - 1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[$"A3:I{rowIndex - 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    package.Save();

                }

                stream.Position = 0;
                _serviceResult.Data = stream;
                return _serviceResult;
            }
            catch (Exception ex)
            {
                _serviceResult.isValid = false;
                _serviceResult.Message = Properties.ResourceVN.Error_Message_UserVN;
                return _serviceResult;
            }

        }
    }

}
