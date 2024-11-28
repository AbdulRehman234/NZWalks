using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Interfaces.Token;
using NZWalks.API.Models.DTO.Auth;
using NZWalks.API.Models.DTO.Login;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;   
            _tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                // add role for user 
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registerd successfully! Please login.");
                    }
                }
            }
            return BadRequest("Something went wrong");
            
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] RequestLoginDto requestLoginDto)
        {
            var identityUser = await _userManager.FindByEmailAsync(requestLoginDto.UserName);
            if(identityUser != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, requestLoginDto.Password);
                if (checkPasswordResult)
                {
                    //first of all get Reole from identity
                    var roles =await _userManager.GetRolesAsync(identityUser);
                    if(roles != null)
                    {
                        //Create Token
                        var jwtToken = _tokenRepository.CreateJWTToken(identityUser, roles.ToList());
                        var response = new LoginResponseDto()
                        {
                            JWTToken = jwtToken,
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("User or password incorrect");
        }
    }
}
