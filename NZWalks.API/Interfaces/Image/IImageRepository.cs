namespace NZWalks.API.Interfaces.Image
{
    public interface IImageRepository
    {
        Task<NZWalks.API.Models.Domian.Image> Upload (NZWalks.API.Models.Domian.Image image);
    }
}
