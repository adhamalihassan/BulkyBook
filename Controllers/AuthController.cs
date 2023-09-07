using BulkyBook.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login()
    {
        // Authenticate user here (check credentials, validate, etc.)
        // Once authenticated, get user information (e.g., userId and username)

        var userId = "user123"; // Get the actual user ID after successful authentication
        var username = "john.doe"; // Get the actual username after successful authentication

        var jwtToken = _authService.GenerateJwtToken(userId, username);

        // Return the JWT token to the client
        return Ok(new { token = jwtToken });
    }
}
