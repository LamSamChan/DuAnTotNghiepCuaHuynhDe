
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services
{
    public class MinuteCoordinateSvc : IMinuteCoordinateSvc
    {
        private readonly ProjectDbContext _context;
        public MinuteCoordinateSvc(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<int> add(PostMinuteCoordinate MCoordinate)
        {
           int ret = 0;
            try{
                MinuteCoordinate add = new MinuteCoordinate()
                {
                    FieldName = MCoordinate.FieldName,
                    X = MCoordinate.X,
                    Y = MCoordinate.Y,
                    SignaturePage = MCoordinate.SignaturePage,
                    TMinuteID = MCoordinate.TMinutetID
                };
                await _context.MinuteCoordinates.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.MinuteCoorID;
            }catch
            {
                return ret;
            }
        }

        public async Task<List<MinuteCoordinate>> getAll()
        {
            return await _context.MinuteCoordinates.ToListAsync();
        }

        public async Task<MinuteCoordinate> getById(int id)
        {
            return await _context.MinuteCoordinates.Where(M => M.MinuteCoorID == id).FirstOrDefaultAsync();
        }

        public Task<List<MinuteCoordinate>> getByTMinute(int id)
        {
            return _context.MinuteCoordinates.Where(M => M.TMinuteID == id).ToListAsync();
        }

        public Task<int> update(PutMinuteCoordinate CCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}