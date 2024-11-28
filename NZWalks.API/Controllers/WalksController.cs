using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomValidations;
using NZWalks.API.Interfaces.Walks;
using NZWalks.API.Mappings;
using NZWalks.API.Models.Domian;
using NZWalks.API.Models.DTO.Walks;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalksRepository _walkRepository;
        public WalksController(IMapper mapper, IWalksRepository walks)
        {
            this._mapper = mapper;
            this._walkRepository = walks;
        }
        [HttpPost]
        [CustomValidationModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto walksRequest)
        {
                //Convert From WalkRequestDto to walks Domain Model using Mapper
                Walk walkDomainModel = _mapper.Map<Walk>(walksRequest);
                await _walkRepository.AddAsync(walkDomainModel);

                //EF Core Smart auto bind id inside domain model so thats way not assign any variable walkDomainModel auto have all value 
                //now convert domain to walksDto 
                return Ok(_mapper.Map<WalksDto>(walkDomainModel));
        }
        //GET : Walks
        //GET : /api/walks?filterOn = Name&filterQuery=Track
        // making Query Paramete make unable if not have value then only get all record if have parameter then filter all record base on parameter
        [HttpGet]
        [Route("getAllbyFiltering")]
        public async Task<IActionResult> GetAllByFiltering([FromQuery] string? filterOn, [FromQuery] string? filterQuery ,[FromQuery] string? sortBy,[FromQuery]bool? isAscending
                                                , [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000 )
        {
            var walkDomainModel = await _walkRepository.GetAllByFilteringAsync(filterOn,filterQuery,sortBy,isAscending ?? true,pageNumber,pageSize);
            //Convert walk Domain Model to Dto using mpper
            return Ok(_mapper.Map<List<WalksDto>>(walkDomainModel));
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //Domain to DTO
            return Ok(_mapper.Map<WalksDto>(walkDomainModel));
        }
        [HttpPut]
        [Route("{id:guid}")]
        [CustomValidationModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto walkRequestDto)
        {

            //Convert From UpdateWalkRequestDto to walk Domain Model
            var walkDomainModel = _mapper.Map<Walk>(walkRequestDto);
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //convert Domain Model to Dto
            return Ok(_mapper.Map<WalksDto>(walkDomainModel));
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteWalkDomain =await _walkRepository.DeleteAsync(id);
            if (deleteWalkDomain == null)
            {
                return NotFound();
            }
            //convert walks domain to Dto
            return Ok(_mapper.Map<WalksDto>(deleteWalkDomain));
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
           var result = await _walkRepository.GetAllAsync();
           return Ok(result);
        }
    }
}
