using Core.Security.Dtos;
using Microsoft.AspNetCore.Mvc;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]UserForLoginDto dto,CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpGet("getallpaginate")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int index, [FromQuery] int size)
    {
        var result = await _authService.GetAllPaginateAsync(index, size);
        return Ok(result);
    }
}