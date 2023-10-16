using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.EmployeeService
{
    public class EmployeeSvc : IEmployeeSvc
    {
        private readonly ProjectDbContext _context;

        public EmployeeSvc(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddNew(Employee employee)
        {
            string isSuccess = null;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            isSuccess = employee.EmployeeId.ToString();
            return isSuccess;
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
                existingEmp.EmployeeId = employee.EmployeeId;
                existingEmp.FullName = employee.FullName;
                existingEmp.Email = employee.Email;
                existingEmp.DateOfBirth = employee.DateOfBirth;
                existingEmp.Gender = employee.Gender;
                existingEmp.PhoneNumber = employee.PhoneNumber;
                existingEmp.Identification = employee.Identification;
                existingEmp.Image = employee.Image;
                existingEmp.Address = employee.Address;
                if (employee.Password == null)
                {
                    existingEmp.Password = existingEmp.Password;
                }
                existingEmp.IsLocked = employee.IsLocked;
                existingEmp.Note = employee.Note;
                existingEmp.SerialPFX = employee.SerialPFX;
                existingEmp.RoleID = employee.RoleID;
                existingEmp.PositionID = employee.PositionID;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = employee.EmployeeId.ToString();
            }
            catch (System.Exception ex)
            {
                return status;
            }

            return status;
        }
    }
}
