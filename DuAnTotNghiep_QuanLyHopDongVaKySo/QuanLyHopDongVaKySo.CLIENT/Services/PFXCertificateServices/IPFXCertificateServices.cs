using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices
{
    public interface IPFXCertificateServices
    {
        Task<List<PFXCertificate>> GetAll();
        //Danh sách sắp hết 
        Task<List<PFXCertificate>> GetAllAboutToExpire();
        //Danh sách hết hạn
        Task<List<PFXCertificate>> GetAllExpire();

        Task<PFXCertificate> GetById(string serial);
        Task<string> Update(PFXCertificate pfxCertificate);

        Task<string> UpdateNotAfter(string serial);

    }
}
