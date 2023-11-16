using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using QuanLyHopDongVaKySo_API.Services.PositionService;
using QuanLyHopDongVaKySo_API.Services.RoleService;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Services.EmployeeService
{
    public class EmployeeSvc : IEmployeeSvc
    {
        private readonly ProjectDbContext _context;
        private readonly IEncodeHelper _encodeHelper;
        private readonly IPFXCertificateSvc _pfxCertificateSvc;
        private readonly IPositionSvc _positionSvc;
        private readonly IRoleSvc _roleSvc;

        public EmployeeSvc(ProjectDbContext context, IEncodeHelper encodeHelper, IPFXCertificateSvc pfxCertificateSvc, IRoleSvc roleSvc, IPositionSvc positionSvc)
        {
            _context = context;
            _encodeHelper = encodeHelper;
            _pfxCertificateSvc = pfxCertificateSvc;
            _roleSvc = roleSvc;
            _positionSvc = positionSvc;
        }

        public async Task<string> AddNew(Employee employee)
        {
            string isSuccess = null;
            string checkFieldExist = await IsFieldExist(employee);
            string checkRoleAndPositon = await IsRoleOrPositonCheck(employee);
            try
            {
                if (String.Compare(checkFieldExist, "0") != 0)
                {
                    return checkFieldExist;
                }
                else if (String.Compare(checkRoleAndPositon, "0") != 0)
                {
                    return checkRoleAndPositon;
                }
                else
                {
                    string passwordPfx = _encodeHelper.Encode(employee.PhoneNumber);
                    string passwordEmp = _encodeHelper.Encode(employee.Password);
                    string serialPFX = await _pfxCertificateSvc.CreatePFXCertificate("TechSeal", employee.FullName, passwordPfx, true);
                    employee.SerialPFX = serialPFX;
                    employee.Password = passwordEmp;
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();
                    isSuccess = employee.EmployeeId.ToString();
                }
            }
            catch (Exception ex)
            {
                return isSuccess;
            }
            return isSuccess;
        }

        public async Task<int?> ChangePassword(ChangePassword changePassword)
        {
            var employee = await GetById(changePassword.EmployeeID);
            if (employee != null)
            {
                if (employee.Password != _encodeHelper.Encode(changePassword.Password))
                {
                    return 0;
                }
                else
                {
                    employee.Password = _encodeHelper.Encode(changePassword.NewPassword);
                    await Update(employee);
                    return 1;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<int> ForgotPassword(string newPassword, string empEmail)
        {
            var employee =  _context.Employees.FirstOrDefault(e => e.Email == empEmail);

            if (employee != null)
            {
                employee.Password = _encodeHelper.Encode(newPassword);
                await Update(employee);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<Employee>> GetAll()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Employee>();
            }
        }

        public async Task<Employee> GetById(string empID)
        {
            try
            {
                return _context.Employees.FirstOrDefault(e => e.EmployeeId == Guid.Parse(empID));
            }
            catch (Exception ex)
            {
                return new Employee();
            }
        }

        public async Task<Employee> GetByEmail(string empEmail)
        {
            try
            {
                return await _context.Employees.FirstOrDefaultAsync (e => e.Email == empEmail);
            }
            catch (Exception ex)
            {
                return new Employee();
            }
        }

        //return -1: trung mail, -2: trung sđth,-3: trung cccd
        public async Task<string> IsFieldExist(Employee employee)
        {
            string phoneNumber = null;

            if (employee.PhoneNumber.StartsWith("+84"))
            {
                phoneNumber = employee.PhoneNumber.Substring(3);
            }
            else
            {
                phoneNumber = employee.PhoneNumber.Substring(1);
            }

            foreach (var emp in await _context.Employees.ToListAsync())
            {
                if (emp.EmployeeId == employee.EmployeeId) continue;

                string existPhoneNumber = null;
                if (emp.PhoneNumber.StartsWith("+84"))
                {
                    existPhoneNumber = emp.PhoneNumber.Substring(3);
                }
                else
                {
                    existPhoneNumber = emp.PhoneNumber.Substring(1);
                }

                if (employee.Email == emp.Email)
                {
                    return "-1";
                }
                else if (phoneNumber == existPhoneNumber)
                {
                    return "-2";
                }
                else if (employee.Identification == emp.Identification)
                {
                    return "-3";
                }
            }

            return "0";
        }

        //return -4: role đã bị ẩn,-5: position bị ẩn, -6: không tồn tại
        public async Task<string> IsRoleOrPositonCheck(Employee employee)
        {
            List<Role>? roleHiddenList = await _roleSvc.GetAllHidden();
            List<Position>? positionHiddenList = await _positionSvc.GetAllHidden();
            List<Role>? roleList = await _roleSvc.GetAll();
            List<Position>? positionList = await _positionSvc.GetAll();

            foreach (var role in roleHiddenList)
            {
                if (employee.RoleID == role.RoleID)
                {
                    return "-4";
                }
            }

            foreach (var positon in positionHiddenList)
            {
                if (employee.PositionID == positon.PositionID)
                {
                    return "-5";
                }
            }

            foreach (var role in roleList)
            {
                if (employee.RoleID == role.RoleID)
                {
                    foreach (var positon in positionList)
                    {
                        if (employee.PositionID == positon.PositionID)
                        {
                            return "0";
                        }
                    }
                }
            }

            return "-6";
        }

        

        public async Task<string> Update(Employee employee)
        {
            string status = null;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingEmp = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingEmp == null)
                {
                    return status;
                }

                // Cập nhật thông tin của đối tượng
                existingEmp.EmployeeId = existingEmp.EmployeeId;
                existingEmp.FullName = employee.FullName;
                existingEmp.Email = employee.Email;
                existingEmp.DateOfBirth = employee.DateOfBirth;
                existingEmp.Gender = employee.Gender;
                existingEmp.PhoneNumber = employee.PhoneNumber;
                existingEmp.Identification = employee.Identification;
                if (employee.Image == null)
                {
                    existingEmp.Image = existingEmp.Image;
                }
                else
                {
                    existingEmp.Image = employee.Image;
                }
                existingEmp.Address = employee.Address;
                existingEmp.Password = existingEmp.Password;
                existingEmp.IsLocked = employee.IsLocked;
                existingEmp.Note = employee.Note;
                existingEmp.SerialPFX = existingEmp.SerialPFX;
                existingEmp.RoleID = employee.RoleID;
                existingEmp.PositionID = employee.PositionID;
                existingEmp.IsFirstLogin = employee.IsFirstLogin;

                string checkFiled = await IsFieldExist(existingEmp);
                string checkHidden = await IsRoleOrPositonCheck(existingEmp);
                if (String.Compare(checkFiled, "0") != 0)
                {
                    return checkFiled;
                }
                else if (String.Compare(checkHidden, "0") != 0)
                {
                    return checkHidden;
                }
                else
                {
                    await _context.SaveChangesAsync();
                    status = employee.EmployeeId.ToString();
                }
            }
            catch (System.Exception ex)
            {
                return status;
            }

            return status;
        }

        public async Task<Employee> GetBySerialPFX(string serial)
        {
            try
            {
                return _context.Employees.FirstOrDefault(e => e.SerialPFX == serial);
            }
            catch (Exception ex)
            {
                return new Employee();
            }
        }
    }
}