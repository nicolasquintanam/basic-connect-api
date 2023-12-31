namespace BasicConnectApi.Test.Controllers;

using BasicConnectApi.Controllers;
using BasicConnectApi.Services;
using BasicConnectApi.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Authorization;

public class AuthControllerTests
{
    private readonly AuthController _controller;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;

    public AuthControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _controller = new AuthController(_userServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public void Login_With_Valid_Credentials_Should_Return_OkResult_With_Token()
    {
        // Arrange
        var validLoginRequest = new LoginRequest { Email = "valid@example.com", Password = "ValidPassword" };
        var userId = It.IsAny<int>();
        var tokenGenerated = "dummyToken";
        _userServiceMock.Setup(x => x.AuthenticateUser(validLoginRequest.Email, validLoginRequest.Password, out userId)).Returns(true);
        _jwtServiceMock.Setup(x => x.GenerateToken(It.IsAny<string>(), null)).Returns(tokenGenerated);

        // Act
        var result = _controller.Login(validLoginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(okResult.Value);
        Assert.True(baseResponse.IsSuccess);
        Assert.NotNull(baseResponse.Data);
        var tokenProperty = baseResponse.Data.GetType().GetProperty("token");
        var tokenValue = tokenProperty.GetValue(baseResponse.Data);
        Assert.NotNull(tokenValue);
        Assert.Equal(tokenValue, tokenGenerated);
    }

    [Fact]
    public void Login_With_Invalid_Credentials_Should_Return_UnauthorizedResult_With_Error_Message()
    {
        // Arrange
        var invalidLoginRequest = new LoginRequest { Email = "invalid@example.com", Password = "InvalidPassword" };
        var userId = It.IsAny<int>();
        _userServiceMock.Setup(x => x.AuthenticateUser(invalidLoginRequest.Email, invalidLoginRequest.Password, out userId)).Returns(false);

        // Act
        var result = _controller.Login(invalidLoginRequest);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var baseResponse = Assert.IsType<BaseResponse>(unauthorizedResult.Value);
        Assert.False(baseResponse.IsSuccess);
        Assert.Equal("Invalid username or password", baseResponse.Message);
    }

    [Fact]
    public void Verify_Logout_Method_Is_Decorated_With_Authorize_Attribute()
    {
        var methodInfo = _controller.GetType().GetMethod("Logout");
        var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
        Assert.True(attributes.Any(), "No AuthorizeAttribute found on Logout method");
    }
}