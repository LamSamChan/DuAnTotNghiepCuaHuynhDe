using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.StampService
{
    public interface IStampSvc
    {
        Task<List<Stamp>> GetAll();
        Task<Stamp> GetById(int id);
        Task<int> AddNew(Stamp stamp);
        Task<int> Update(Stamp stamp);
        Task<int> Delete(int id);

    }
}
