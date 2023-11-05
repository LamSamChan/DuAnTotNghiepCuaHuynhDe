﻿using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Services
{
    public interface IAuthServices
    {
        Task<Employee> Login(ViewLogin viewLogin);
    }
    public class AuthServices : IAuthServices
    {
        private readonly ProjectDbContext _context;
        private readonly IEncodeHelper _encodeHelper;
        public AuthServices(ProjectDbContext context, IEncodeHelper encodeHelper)
        {
            _context = context;
            _encodeHelper = encodeHelper;
        }
        public async Task<Employee> Login(ViewLogin viewLogin)
        {
            var login = _context.Employees.FirstOrDefault(e => e.Email.Equals(viewLogin.Email) && e.Password.Equals(_encodeHelper.Encode(viewLogin.Password)));

            return login;
        }
    }
}