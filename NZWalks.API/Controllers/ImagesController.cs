using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Interfaces.Image;
using NZWalks.API.Models.Domian;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _image;

        public ImagesController(IImageRepository image)
        {
            this._image = image;
        }
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto requestDto)
        {
            this.ValidateFileUpload(requestDto);
            if(ModelState.IsValid)
            {
                //Convert DTO to Domain Model
                var imageDomainModel = new Image()
                {
                    File = requestDto.File,
                    FileExtension = Path.GetExtension(requestDto.File.FileName),
                    FileSizeInBytes = requestDto.File.Length,
                    FileName = requestDto.FileName,
                    FileDescription = requestDto.FileDescription,
                };
                //Repository to upload image 
               await _image.Upload(imageDomainModel);

               return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);

        }
        private void ValidateFileUpload(ImageUploadRequestDto requestDto)
        {
            // add restriction on file ha this type of extention 
            var allowExtention = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtention.Contains(Path.GetExtension(requestDto.File.FileName))) //if Extentionnot get in allowExtention
            {
                ModelState.AddModelError("file", "Unsupported file extention");
            }
            if(requestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more then 10MB , Please Upload a smaller file size");
            }
        }
    }
}
