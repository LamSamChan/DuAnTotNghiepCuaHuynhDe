
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services
{
    public interface IMinuteCoordinateSvc
    {
        Task<List<MinuteCoordinate>> getAll();
        Task<MinuteCoordinate> getById(int id);
        Task<int> add(PostMinuteCoordinate CCoordinate);
        Task<int> update(PutMinuteCoordinate CCoordinate);
        Task<List<MinuteCoordinate>> getByTMinute(int id);
    }
}