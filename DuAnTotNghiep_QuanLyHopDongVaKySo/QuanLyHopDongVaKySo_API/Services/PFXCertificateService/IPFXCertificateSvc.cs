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

        //tạo file -> lưu thông tin vào dtb
        Task<string> CreatePFXCertificate(string issuerName, string subjectName, string pfxPassword);

        //cập nhật lại thợi gian hiệu lực đối với file -> lưu thông tin vào dtb
        Task<string> UpdateNotAfter(string pfxFilePath, string password);

        Task<string> GetInfoFormPFXCertificate(PFXCertificate pfxCertificate);

        //cập nhật vô database
        Task<string> AddInfoToDatabase(PFXCertificate pfxCertificate);

        //cập nhật vô database
        Task<string> UpdateInfoToDatabase(PFXCertificate pfxCertificate);

        //Task<int> SignContract();
    }
}
