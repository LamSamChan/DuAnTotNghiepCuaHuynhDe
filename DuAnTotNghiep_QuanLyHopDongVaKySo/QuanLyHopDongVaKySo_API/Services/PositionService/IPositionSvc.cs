using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PositionService
{
    public interface IPositionSvc
    {
        Task<List<Position>> GetAll();
        Task<List<Position>> GetAllNotHidden();
        Task<List<Position>> GetAllHidden();
        Task<Position> GetById(int positionId);
        Task<int> AddNew(Position position);
        Task<int> Update(Position position);
    }
}
