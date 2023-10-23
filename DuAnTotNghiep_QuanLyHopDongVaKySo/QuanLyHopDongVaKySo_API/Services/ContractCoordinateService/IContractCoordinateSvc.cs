

using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services
{
    public interface IContractCoordinateSvc
    {
        Task<List<ContractCoordinate>> getAll();
        Task<ContractCoordinate> getById(int id);
        Task<int> add(PostContractCoordinate CCoordinate);
        Task<int> update(PutContractCoordinate CCoordinate);
        Task<List<ContractCoordinate>> getByTContract(int id);
    }   
}