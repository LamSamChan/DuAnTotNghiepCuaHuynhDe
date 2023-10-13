using Humanizer;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PFXCertificateService
{
    public interface IPFXCertificateSvc
    {
        Task<List<PFXCertificate>> GetAll();
        //Danh sách sắp hết 
        Task<List<PFXCertificate>> GetAllAboutToExpire();
        //Danh sách hết hạn
        Task<List<PFXCertificate>> GetAllExpire();
        Task<PFXCertificate> GetById(string serial);
        Task<string> AddNew(PFXCertificate certificate);
        Task<string> UpdateNotAfter(PFXCertificate certificate);
        Task<int> SignContract();
    }
}
