using NZWalks.API.Models.Domian;
using NZWalks.API.Models.DTO.Difficulty;

namespace NZWalks.API.Models.DTO.Walks
{
    public class WalksDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }

        //Nevigation Id not required here because its a return Complete Region and Difficulty Object

    }
}
