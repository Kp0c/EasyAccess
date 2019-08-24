using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokensController : ControllerBase
    {
        ITokenService _tokenService;

        public TokensController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult UpserNewToken([FromBody] FirebaseToken firebaseToken)
        {
            try
            {
                _tokenService.UpsertNewToken(firebaseToken);
                return Ok();
            }
            catch(AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }

}
