using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Interfaces;
using NZWalks.API.Models.Domian;

namespace NZWalks.API.Repositories.RegionDir
{
    public class RegionRepository : IRegion
    {

        /* Repository always handle Db Interction And Bd call seprate from UI interface 
        * Repository class handle db call, code speration , execution fast, readable code  , code decoupling
        * Always inject db class in repository 
        */

        private readonly NZWalksDbContext _context;

        public RegionRepository(NZWalksDbContext nZWalksDb)
        {
            _context = nZWalksDb;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await GetByIdAsync(id);
            if (existingRegion == null)
            {
                return null;
            }
            _context.Regions.Remove(existingRegion);
            await _context.SaveChangesAsync();
            return existingRegion;
        }


        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _context.Regions.FindAsync(id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await GetByIdAsync(id);
            if (existingRegion == null)
            {
                return null;
            }
            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await _context.SaveChangesAsync();
            return existingRegion;
        }
    }
}
