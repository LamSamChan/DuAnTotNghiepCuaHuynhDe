// Ignore Spelling: auth

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Services;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices;
using QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices;
using API= QuanLyHopDongVaKySo_API.Models;
using Azure;
using QuanLyHopDongVaKySo.CLIENT.Models;
using RestSharp.Serializers;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class VerifyController : Controller
    {
        private readonly IAuthServices _authServices;
        private readonly IRoleService _roleService;
        private readonly IPasswordService _passwordService;
        private readonly IHistoryEmpSvc _historyEmpSvc;
        private readonly IEmployeeService _employeeSvc;

        public VerifyController(IAuthServices authServices, IRoleService roleService, IPasswordService passwordService, IHistoryEmpSvc historyEmpSvc, IEmployeeService employeeSvc)
        {
            _authServices = authServices;
            _roleService = roleService;
            _passwordService = passwordService;
            _historyEmpSvc = historyEmpSvc;
            _employeeSvc = employeeSvc;
        }

        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
            if (String.IsNullOrEmpty(role))
            {
                ViewBag.SweetType = TempData["SweetType"];
                ViewBag.SweetIcon = TempData["SweetIcon"];
                ViewBag.SweetTitle = TempData["SweetTitle"];
                return View();
            }
            else
            {
                if (role == "Admin")
                {
                    return RedirectToAction("Index1", "Admin");
                }
                else if (role == "Giám đốc")
                {
                    return RedirectToAction("Index", "Director");
                }
                else if (role == "Nhân viên kinh doanh")
                {
                    return RedirectToAction("Index", "BusinessStaff");
                }
                else
                {
                    return RedirectToAction("Index", "InstallStaff");
                }
            }
        }
      

        public IActionResult ResetPass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOTP([FromBody]ForgotPassword forgotPassword)
        {
            var respone = await _passwordService.GetOTPForgotAsync(forgotPassword);
            if (respone != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] string comfirmOTP)
        {
            var respone = await _passwordService.ForgotPasswordAsync(comfirmOTP);
            if (respone != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        public async Task<IActionResult> Login(VMLogin login)
        {
            var result = await _authServices.Login(login);


            if (result != null)
            {
                HttpContext.Session.SetString("token", result);
                var reponse = await _employeeSvc.GetEmployeeByEmail(login.Email);
                HttpContext.Session.SetString(SessionKey.Employee.EmployeeContext,JsonConvert.SerializeObject(reponse));
                HttpContext.Session.SetString(SessionKey.Employee.EmployeeID, reponse.EmployeeId.ToString());
                HttpContext.Session.SetString(SessionKey.Employee.Role, _roleService.GetRoleByIdAsync(reponse.RoleID).Result.RoleName);
                string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
                if (role == "Admin")
                {
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{reponse.FullName} - ID:{reponse.EmployeeId.ToString().Substring(0, 8)} đã đăng nhập vào hệ thống.",
                        EmployeeID = reponse.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);

                  
                    if (reponse.IsFirstLogin)
                    {
                        TempData["SweetType"] = "success";
                        TempData["SweetIcon"] = "success";
                        TempData["SweetTitle"] = $"Đăng nhập {role} thành công... Bạn hãy thay đổi mật khẩu !!";
                        return RedirectToAction("Index1", "Admin");
                    }

                   
                    return RedirectToAction("Index", "Admin");
                }
                else if (role == "Giám đốc")
                {
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{reponse.FullName} - ID:{reponse.EmployeeId.ToString().Substring(0, 8)} đã đăng nhập vào hệ thống.",
                        EmployeeID = reponse.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);

                   
                    if (reponse.IsFirstLogin)
                    {
                        TempData["SweetType"] = "success";
                        TempData["SweetIcon"] = "success";
                        TempData["SweetTitle"] = $"Đăng nhập {role} thành công... Bạn hãy thay đổi mật khẩu !!";

                        return RedirectToAction("ChangePass", "Director");
                    }
                    
                    return RedirectToAction("Index", "Director");
                }
                else if (role =="Nhân viên kinh doanh")
                {
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{reponse.FullName} - ID:{reponse.EmployeeId.ToString().Substring(0, 8)} đã đăng nhập vào hệ thống.",
                        EmployeeID = reponse.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);

                   
                    if (reponse.IsFirstLogin)
                    {
                        TempData["SweetType"] = "success";
                        TempData["SweetIcon"] = "success";
                        TempData["SweetTitle"] = $"Đăng nhập {role} thành công... Bạn hãy thay đổi mật khẩu !!";
                        return RedirectToAction("ChangePass", "BusinessStaff");
                    }

                    return RedirectToAction("Index", "BusinessStaff");
                }
                else
                {
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{reponse.FullName} - ID:{reponse.EmployeeId.ToString().Substring(0, 8)} đã đăng nhập vào hệ thống.",
                        EmployeeID = reponse.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);

              
                    if (reponse.IsFirstLogin)
                    {
                        TempData["SweetType"] = "success";
                        TempData["SweetIcon"] = "success";
                        TempData["SweetTitle"] = $"Đăng nhập {role} thành công... Bạn hãy thay đổi mật khẩu !!";
                        return RedirectToAction("ChangePass", "InstallStaff");
                    }

                    return RedirectToAction("Index", "InstallStaff");
                } 
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Đăng nhập thất bại, kiểm tra lại thông tin đăng nhập !!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
            var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
            API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
            {
                OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã đăng xuất khỏi hệ thống.",
                EmployeeID = employeeDoing.EmployeeId
            };
            await _historyEmpSvc.AddNew(historyEmp);

            HttpContext.Session.Remove(SessionKey.Employee.EmployeeID);
            HttpContext.Session.Remove(SessionKey.Employee.EmployeeContext);
            HttpContext.Session.Remove(SessionKey.Employee.Role);
            HttpContext.Session.Remove(SessionKey.Customer.CustomerID);
            HttpContext.Session.Remove(SessionKey.Customer.CustomerContext);
            HttpContext.Session.Remove(SessionKey.PFXCertificate.Serial);
            HttpContext.Session.Remove(SessionKey.PedningContract.PContractID);
            HttpContext.Session.Remove(SessionKey.PedningMinute.PMinuteID);
            HttpContext.Session.Remove("token");


            TempData["SweetType"] = "success";
            TempData["SweetIcon"] = "success";
            TempData["SweetTitle"] = $"{role} đã đăng xuất thành công.";
            return RedirectToAction("Index");
        }
    }
}
