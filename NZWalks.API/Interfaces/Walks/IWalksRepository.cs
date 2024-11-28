using NZWalks.API.Interfaces.Generic;
using NZWalks.API.Models.Domian;

namespace NZWalks.API.Interfaces.Walks
{
    public interface IWalksRepository : IGenericRepository<Walk>
    { 
        Task<List<Walk>> GetAllByFilteringAsync(string? filterOn = null,string? filterQuery = null
                                     ,string? sortBy = null, bool isAscending = true,int pageNumber = 1,int pageSize = 1000);
    }
}
