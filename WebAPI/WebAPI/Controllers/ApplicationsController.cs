using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Dtos;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        IApplicationService _applicationService;
        IMapper _mapper;
        readonly AppSettings _appSettings;

        public ApplicationsController(IApplicationService applicationService,
                                      IMapper mapper,
                                      IOptions<AppSettings> appSettings)
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetApplications(string userId)
        {
            try
            {
                var applications = _applicationService.GetApplications(userId);

                var applicationsDtos = _mapper.Map<ICollection<ApplicationDto>>(applications);
                return Ok(applicationsDtos);
            }
            catch (AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string userId, string id)
        {
            try
            {
                var application = _applicationService.GetById(userId, id);
                var applicationDto = _mapper.Map<ApplicationDto>(application);

                return Ok(applicationDto);
            }
            catch(AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddApplication(string userId, [FromBody]ApplicationDto applicationDto)
        {
            var application = _mapper.Map<Application>(applicationDto);

            try
            {
                _applicationService.AddApplication(userId, application);
                return Ok();
            }
            catch(AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string userId, string id)
        {
            try
            {
                _applicationService.Delete(userId, id);
                return Ok();
            }
            catch(AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}/authenticate")]
        public IActionResult Authenticate(string userId, string id, [FromQuery] string name)
        {
            try
            {
                if (_applicationService.Authenticate(userId, id, name))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, userId)
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
            catch(AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/[controller]/{id}/complete/{authId}")]
        public IActionResult CompleteAuthentication(string id, string authId)
        {
            try
            {
                _applicationService.CompleteAuthentication(id, authId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
