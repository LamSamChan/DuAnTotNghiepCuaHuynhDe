﻿using QuanLyHopDongVaKySo.CLIENT.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.StampService
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
