using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomValidations;
using NZWalks.API.Data;
using NZWalks.API.Interfaces;
using NZWalks.API.Mappings;
using NZWalks.API.Models.Domian;
using NZWalks.API.Models.DTO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RegionsController : ControllerBase
    {
        private readonly IRegion _regionRepo;

        public IMapper _mapper { get; }

        public RegionsController(IRegion region,IMapper mapper)
        {
            _regionRepo = region;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "READER")]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain =await _regionRepo.GetAllAsync();
            //Best Paractices is that return DTO instaed of Domain Modle
            //Convert Region to RegionDTO Using AutoMapper
            var regionDto = _mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(regionDto);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "READER,WRITER")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var regionDomain = await _regionRepo.GetByIdAsync(id);
            if (regionDomain == null)
                return NotFound();

            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }
        [HttpPost]
        [Authorize(Roles = "WRITER")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                //First Of All Convert AddRegionRequestDto to into Region Domain Model

                var regionDomain = _mapper.Map<Region>(addRegionRequestDto);
                await _regionRepo.CreateAsync(regionDomain);

                //Now Used double return action method which is return Response and also creating object which
                //is created now Using CreateAtAction
                //Syntax Take action name which you want to perform , EntityId,ReturningObjct which type of object 
                //do you want to return 
                //CreatedAtAction return response state added successfully and call getById method and return
                //its value also

                //convert region domian into Region dto
                var regionDto = _mapper.Map<RegionDto>(regionDomain);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut]
        [Route("{id:guid}")]
        [CustomValidationModel] //this is a custom validation that remove extra if else code from controller you can compare creat update action with each other
        [Authorize(Roles = "WRITER")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegion)
        {
                var region = _mapper.Map<Region>(updateRegion);
                var regionObj = await _regionRepo.UpdateAsync(id, region);
                if (regionObj == null)
                    return NotFound();
                //convert Region Model into region dto
                return Ok(_mapper.Map<RegionDto>(regionObj));
        }
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "WRITER")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region =await _regionRepo.DeleteAsync(id);
            if (region == null)
                return NotFound();
            //if you want to return delete region
            //Convert Region Model to Region Dto
            return Ok(_mapper.Map<RegionDto>(region));
        }

        //[HttpGet]
        //[Route("mapper")]
        //public IActionResult TestAutoMapper()
        //{

        //    DestinationClass destinationMapperObj = _mapper.Map<DestinationClass>(new SourcClass());
        //    SourcClass sourceName = new SourcClass()
        //    {
        //        SourceName = ""
        //    };
        //    var sourceMapperObj = _mapper.Map<SourcClass>(destinationMapperObj);
        //    return Ok();

        //    //Syntax _mapper.Map<Destination>(Sources);
        //    //if you configur like this 
        //    /* public AutoMapperProfile()
        //     {
        //        CreateMap<SourcClass, DestinationClass>().ForMember(x => x.DestinationName, opt => opt.MapFrom(y => y.SourceName)).ReverseMap();
        //     } 
        //        then type should be match if your type not match then using ReversMap its give you revers Mapping You can used condition here 
        //    */

        //foreach (var regionDomain in regionsDomain)
            //{
            //    regionDto.Add(new RegionDto
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            //}  this is a mapping which is lot of a code which is not a good if i have 1000 of property then its take much time so thats way used auto mapper 
        //}
    }
}
