namespace BasicConnectApi.Custom;

using BasicConnectApi.Models;
using BasicConnectApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public sealed class AccessTokenAuthorizeAttribute() : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var resultUnauthorized = new UnauthorizedObjectResult(new BaseResponse(false, "Invalid token. Please log in again."));
        var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();
        if (context is null)
            return;
        if (context.HttpContext.User.Identity is null)
            return;

        var accessToken = jwtService.GetTokenFromAuthorizationHeader(context.HttpContext.Request.Headers);
        if (accessToken is null)
        {
            context.Result = resultUnauthorized;
            return;
        }

        if (!jwtService.ValidateAccessToken(accessToken))
        {
            context.Result = resultUnauthorized;
            return;
        }
    }
}