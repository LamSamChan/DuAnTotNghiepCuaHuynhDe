using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PFXCertificateService
{
    public class PFXCertificateSvc : IPFXCertificateSvc
    {
        public Task<string> AddNew(PFXCertificate certificate)
        {
            throw new NotImplementedException();
        }

        public Task<List<PFXCertificate>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<PFXCertificate>> GetAllAboutToExpire()
        {
            throw new NotImplementedException();
        }

        public Task<List<PFXCertificate>> GetAllExpire()
        {
            throw new NotImplementedException();
        }

        public Task<PFXCertificate> GetById(string serial)
        {
            throw new NotImplementedException();
        }

        public Task<int> SignContract()
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateNotAfter(PFXCertificate certificate)
        {
            throw new NotImplementedException();
        }
    }
}
