using System;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PendingContractService
{
    public interface IPendingContractSvc
    {
        Task<List<PendingContract>> getPContractsAsnyc();
        Task<PendingContract> getPContractAsnyc(int id);
        Task<string> addPContractAsnyc(PostPendingContract PContract);
        Task<string> updatePContractAsnyc(PutPendingContract PContract);
        Task<bool> deletePContractAsnyc(int id);
        Task<int> updatePContractFile(int id, string File);
    }
}
