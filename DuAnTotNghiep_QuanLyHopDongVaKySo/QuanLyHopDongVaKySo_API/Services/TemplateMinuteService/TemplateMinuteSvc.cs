using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
namespace QuanLyHopDongVaKySo_API.Services.TemplateMinuteService
{
    public class TemplateMinuteSvc : ITemplateMinuteSvc
    {
        private readonly ProjectDbContext _context;
        private readonly IUploadImageHelper _helpers;

        public TemplateMinuteSvc (ProjectDbContext context,IUploadImageHelper helpers)
        {
            _context = context;
            _helpers = helpers;
        }
        public async Task<int> addTMinuteAsnyc(PostTMinute tMinute)
        {
            try{
                if(tMinute.File != null)
                {
                    _helpers.UploadImage(tMinute.File,"AppData","TMinutes");
                }
                TemplateMinute add = new TemplateMinute
                {
                    DateAdded = DateTime.Now,
                    TMinuteName =  tMinute.TMinuteName,
                    TMinuteFile = @"..\..\..\AppData\TMinutes\"+tMinute.File.FileName,
                    jsonCustomerZone = tMinute.jsonCustomerZone,
                    jsonIntallationZone = tMinute.jsonDirectorZone
                };
                await _context.TemplateMinutes.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.TMinuteID;
            }catch{
                return 0;
            }
        }

        public async Task<bool> deleteTMinueAsnyc(int id)
        {
            var delete = await getTMinuteAsnyc(id);
            if(delete != null)
            {
                _context.TemplateMinutes.Remove(delete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<TemplateMinute> getTMinuteAsnyc(int id)
        {
            return await _context.TemplateMinutes.Where(M => M.TMinuteID == id).FirstOrDefaultAsync();
        }

        public async Task<List<TemplateMinute>> getTMinutesAsnyc()
        {
            return await _context.TemplateMinutes.ToListAsync();
        }

        public async Task<int> updateTMinuteAsnyc(PutTMinute tMinute)
        {
            try{
                var update = await getTMinuteAsnyc(tMinute.TMinuteID);
                if(tMinute.File != null)
                {
                    _helpers.UploadImage(tMinute.File,"Appdata","TMinutes");
                }
                if(update != null)
                {
                    update.TMinuteName = tMinute.TMinuteName;
                    update.TMinuteFile = @"..\..\..\AppData\TContracts\"+tMinute.File.FileName;
                    update.DateAdded = DateTime.Now;
                    update.jsonCustomerZone = tMinute.jsonCustomerZone;
                    update.jsonIntallationZone = tMinute.jsonDirectorZone;
                    _context.TemplateMinutes.Update(update);
                    await _context.SaveChangesAsync();
                    return update.TMinuteID;
                }
                return 0;
            }catch{
                return 0;
            }
        }
    }
}
