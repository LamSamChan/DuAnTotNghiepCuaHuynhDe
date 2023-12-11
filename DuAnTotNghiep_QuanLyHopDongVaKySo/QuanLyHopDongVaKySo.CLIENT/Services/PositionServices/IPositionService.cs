using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PositionServices
{
    public interface IPositionService
    {
        Task<List<Position>> GetAllPositionsAsync();
        Task<List<Position>> GetAllNotHidden();
        Task<Position> GetPositionByIdAsync(int positionId);
        Task<int> AddPositionAsync(Position position);
        Task<int> UpdatePositionAsync(Position position);
    }
}
