using NZWalks.API.Models.Domian;

namespace NZWalks.API.Interfaces
{
    public interface IRegion
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);  //nullable because its may return or not 
        Task<Region?> UpdateAsync(Guid id , Region region);
        Task<Region> CreateAsync(Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
