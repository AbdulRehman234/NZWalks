using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Interfaces.Walks;
using NZWalks.API.Models.Domian;
using NZWalks.API.Repositories.Generic;

namespace NZWalks.API.Repositories.Walks
{
    public class WalkRepository : GenericRepository<Walk> , IWalksRepository
    {
        private readonly NZWalksDbContext _dbcontext;

        public WalkRepository(NZWalksDbContext context) : base(context)
        {
            this._dbcontext = context;
        }

        public async Task<List<Walk>> GetAllByFilteringAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true
                                                             , int pageNumber = 1, int pageSize = 1000)
        {
            //now using with query parameter retreve as Queryable
            var walks = _dbcontext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            /* 1) Appling Filterin */
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false) //means these fields not null or empty 
            {
                //Know check which proprty you want to filter means if you want Name filter Customer user send Description to resrtict this issue
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            /* 1) Appling Sorting */
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
            }
            /* 1)  Pagination Formula */
            var skipResult = (pageNumber - 1) * pageSize;
            //finel Result apply
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();

            //return await _context.Walks.Include("Difficulty").Include("Region").ToListAsync();
            // return await _context.Walks.Include(x=>x.Region).Include(x => x.Difficulty).ToListAsync(); We canUsed These way for nevigation property
        }




        //public async Task<Walk> CreateAsync(Walk walk)
        //{
        //    await _context.Walks.AddAsync(walk);
        //    await _context.SaveChangesAsync();
        //    return walk;
        //}

        //public async Task<Walk?> DeleteAsync(Guid id)
        //{
        //   var existingWalk = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
        //    if(existingWalk == null)
        //    {
        //        return null;
        //    }
        //    _context.Walks.Remove(existingWalk);
        //    await _context.SaveChangesAsync();
        //    return existingWalk;
        //}



        //public async Task<Walk?> GetByIdAsync(Guid id)
        //{
        //    return await _context.Walks.Include(x=>x.Region).Include(x=>x.Difficulty).FirstOrDefaultAsync(x=>x.Id == id);
        //}

        //public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        //{
        //    var existingWalk = await _dbcontext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        //    if (existingWalk == null)
        //    {
        //        return null;
        //    }
        //    existingWalk.Id = id;
        //    existingWalk.DifficultyId = walk.DifficultyId;
        //    existingWalk.RegionId = walk.RegionId;
        //    existingWalk.WalkImageUrl = walk.WalkImageUrl;
        //    existingWalk.Description = walk.Description;
        //    existingWalk.Name = walk.Name;
        //    existingWalk.LengthInKm = walk.LengthInKm;
        //    await _context.SaveChangesAsync();
        //    return existingWalk;
        //}
    }
}
