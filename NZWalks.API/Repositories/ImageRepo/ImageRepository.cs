using NZWalks.API.Data;
using NZWalks.API.Interfaces.Image;
using NZWalks.API.Models.Domian;
using System.Security.AccessControl;

namespace NZWalks.API.Repositories.ImageRepo
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDbContext _context;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,NZWalksDbContext context)
        {
            this._webHostEnvironment = webHostEnvironment; // this is use here because for localpath of Environment image folder where from im get file  
            this._httpContextAccessor = httpContextAccessor;
            this._context = context;
        }
        public async Task<Image> Upload(Image image)
        {
            //_webHostEnvironment.ContentRootPath API Solution Path then Image Folder then fileName + Extention
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Image", 
                $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            /*http://localhost:1234/images/image.jpg */
            //now you dont know what is your local path server path then get dynamicaly through httpContextAccesser inject in program.cs
            //_httpContextAccessor.HttpContext.Request.Scheme = return http or https 
            //_httpContextAccessor.HttpContext.Request.Host  = return localhost or live server host
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}" +
                              $"/Image/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;   
            //add image to image table 
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }
    }
}
