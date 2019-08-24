using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Dtos;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IMapper _mapper;
        readonly AppSettings _appSettings;

        public UsersController(IUserService userService,
                               IMapper mapper,
                               IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet("{username}")]
        public IActionResult GetUser(string username)
        {
            try
            {
                return Ok(_userService.GetByUsername(username));
            }
            catch (AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("authenticateByEmail/{username}")]
        public IActionResult Authenticate(string username, [FromQuery] string authName)
        {
            try
            {
                if(_userService.AuthenticateByEmail(username, authName))
                {
                    var user = _userService.GetByUsername(username);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id)
                        }),
                        Expires = DateTime.UtcNow.AddMonths(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(tokenString);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserToRegisterDto userDto)
        {
            var user = _mapper.Map<UserToRegister>(userDto);

            try
            {
                return Ok(_userService.Register(user));
            }
            catch(AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("register/{id}")]
        public IActionResult CompleteRegistration(string id)
        {
            try
            {
                var user = _userService.CompleteRegistration(id);
                return Ok(user);
            }
            catch (AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}