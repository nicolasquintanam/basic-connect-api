namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;
using BasicConnectApi.Filters;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/v1/[controller]")]
[ServiceFilter(typeof(ValidationFilter))]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public AuthController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!_userService.AuthenticateUser(request.Email, request.Password, out int userId))
            return Unauthorized(new BaseResponse(false, "Invalid username or password"));

        var token = _jwtService.GenerateToken(userId.ToString());
        return Ok(new BaseResponse(true) { Data = new { token } });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var token = _jwtService.GetTokenFromAuthorizationHeader(HttpContext.Request.Headers);
        if (token is not null)
            _jwtService.RevokeToken(token);

        return Ok(new BaseResponse(true));
    }
}
