
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services
{
    public class ContractCoordinateSvc : IContractCoordinateSvc
    {
        private readonly ProjectDbContext _context;
        public ContractCoordinateSvc(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<int> add(PostContractCoordinate CCoordinate)
        {
            int ret = 0;
            try{
                ContractCoordinate add = new ContractCoordinate()
                {
                    FieldName = CCoordinate.FieldName,
                    X = CCoordinate.X,
                    Y = CCoordinate.Y,
                    SignaturePage = CCoordinate.SignaturePage,
                    TContractID = CCoordinate.TContractID
                };
                await _context.ContractCoordinates.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.ContractCoorID;
            }catch
            {
                return ret;
            }
        }

        public async Task<ContractCoordinate> getById(int id)
        {
            return await _context.ContractCoordinates.Where(c => c.ContractCoorID == id).FirstOrDefaultAsync();
        }

        public async Task<List<ContractCoordinate>> getAll()
        {
            return await _context.ContractCoordinates.ToListAsync();
        }

        public async Task<int> update(PutContractCoordinate CCoordinate)
        {
            int ret = 0;
            var update = await getById(CCoordinate.ContractCoorID);
            try{
                if(update != null)
                {
                    update.FieldName = CCoordinate.FieldName;
                    update.X = CCoordinate.X;
                    update.Y = CCoordinate.Y;
                    update.TContractID = CCoordinate.TContractID;
                }
                _context.ContractCoordinates.Update(update);
                await _context.SaveChangesAsync();
                return update.ContractCoorID;
            }catch
            {
                return ret;
            }
        }

        public async Task<List<ContractCoordinate>> getByTContract(int id)
        {
            return await _context.ContractCoordinates.Where(c => c.TContractID == id).ToListAsync();
        }
    }

}