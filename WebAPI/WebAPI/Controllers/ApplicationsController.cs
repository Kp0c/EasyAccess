using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public ApplicationsController(IApplicationService applicationService,
                                      IMapper mapper)
        {
            _applicationService = applicationService;
            _mapper = mapper;
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

        [HttpPut("{id}")]
        public IActionResult Update(string userId, string id, [FromBody]ApplicationDto applicationDto)
        {
            var application = _mapper.Map<Application>(applicationDto);
            application.Id = id;

            try
            {
                _applicationService.Update(userId, application);
                return Ok();
            }
            catch (AppException ex)
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

        [HttpGet("{id}/authenticate")]
        public IActionResult Authenticate(string userId, string id)
        {
            try
            {
                _applicationService.Authenticate(userId, id);
                return Ok();
            }
            catch(AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
