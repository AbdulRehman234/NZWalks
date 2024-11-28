using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be Minimum of 3 Character ")]
        [MaxLength(3, ErrorMessage = "Code has to be a Maximum of 3 Character")]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
