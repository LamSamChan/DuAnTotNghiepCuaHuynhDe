using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_CLIENT.Services.PositionServices
{
    public interface IPositionService
    {
        Task<IEnumerable<Position>> GetAllPositionsAsync();
        Task<Position> GetPositionByIdAsync(int id);
        Task<int> AddPositionAsync(Position position);
        Task<int> UpdatePositionAsync(Position position);
    }
}
