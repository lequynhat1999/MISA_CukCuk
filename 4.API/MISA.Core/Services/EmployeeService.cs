using Microsoft.AspNetCore.Http;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region DECLARE
        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _serviceResult = new ServiceResult();
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Methods
        public ServiceResult ExportEmployee()
        {
            var employees = _employeeRepository.Get();
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(employees, true);
                package.Save();
            }
            _serviceResult.Data = stream;
            return _serviceResult;
        }

        public ServiceResult ImportEmployee(IFormFile formFile)
        {
            // Check file null
            if (formFile == null || formFile.Length <= 0)
            {
                _serviceResult.Message = "File bị trống, xin vui lòng gửi lại file";
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                return _serviceResult;
            }

            // Check file không đúng định dạng
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                _serviceResult.Message = "File gửi lên không đúng định dạng";
                _serviceResult.MISACode = MISAEnum.EnumServiceResult.BadRequest;
                return _serviceResult;
            }

            HashSet<string> contain = new HashSet<string>();
            var employees = new List<Tuple<Employee, List<string>>>();
            using (var stream = new MemoryStream())
            {
                formFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 3; row < rowCount; row++)
                    {
                        // lấy ra value
                        var employeeId = Guid.NewGuid();
                        var employeeCode = worksheet.Cells[row, 1].Value;
                        var fullName = worksheet.Cells[row, 2].Value;
                        var phoneNumber = worksheet.Cells[row, 5].Value;
                        var personalTaxCode = worksheet.Cells[row, 8].Value;
                        var email = worksheet.Cells[row, 9].Value;
                        var dateOfBirth = worksheet.Cells[row, 6].Value;

                        // check null
                        var employee = new Employee
                        {
                            EmployeeId = employeeId,
                            EmployeeCode = (employeeCode != null) ? employeeCode.ToString().Trim() : string.Empty,
                            FullName = (fullName != null) ? fullName.ToString().Trim() : string.Empty,
                            PhoneNumber = (phoneNumber != null) ? phoneNumber.ToString().Trim() : string.Empty,
                            PersonalTaxCode = (personalTaxCode != null) ? personalTaxCode.ToString().Trim() : string.Empty,
                            Email = (email != null) ? email.ToString().Trim() : string.Empty,
                            DateOfBirth = (dateOfBirth != null) ? FormatDateTime(dateOfBirth.ToString().Trim()) : null,
                        };

                        // validate trong file
                        if (!contain.Contains(employee.EmployeeCode) && !contain.Contains(employee.Email) && !contain.Contains(employee.PhoneNumber))
                        {
                            contain.Add(employee.EmployeeCode);
                            contain.Add(employee.PhoneNumber);
                            contain.Add(employee.Email);
                            employees.Add(Tuple.Create(employee, new List<string>()));
                        }
                        else
                        {
                            //if (contain.Contains(employee.EmployeeCode))
                            //{
                            //    employee.ImportArrError.Add("Mã nhân viên đã tồn tại trong file");
                            //}
                            //if (contain.Contains(employee.Email))
                            //{
                            //    employee.ImportArrError.Add("Email đã tồn tại trong file");
                            //}
                            //if (contain.Contains(employee.PhoneNumber))
                            //{
                            //    employee.ImportArrError.Add("SDT đã tồn tại trong file");
                            //}
                            //employees.Add(Tuple.Create(employee, employee.ImportArrError));
                            List<string> notifications = new List<string>();
                            if (contain.Contains(employee.EmployeeCode))
                            {
                                notifications.Add("Mã nhân viên đã tồn tại trong file");
                            }
                            if (contain.Contains(employee.Email))
                            {
                                notifications.Add("Email đã tồn tại trong file");
                            }
                            if (contain.Contains(employee.PhoneNumber))
                            {
                                notifications.Add("SDT đã tồn tại trong file");
                            }
                            employees.Add(Tuple.Create(employee, notifications));
                        }
                    }
                    //contain.Clear();
                }
            }
            _serviceResult.Data = employees;
            return _serviceResult;
        }

        /// <summary>
        /// Format datetime
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private static DateTime? FormatDateTime(string dateString)
        {
            var str = dateString.Replace('-', '/').Split("/");
            switch (str.Length)
            {
                case 1:
                    return DateTime.Parse($"{str[0]}/01/01");
                case 2:
                    return DateTime.Parse($"{str[1]}/{str[0]}/01");
                case 3:
                    return DateTime.Parse($"{str[2]}/{str[1]}/{str[0]}");
                default:
                    return null;
            }
        }



        #endregion
    }
}
