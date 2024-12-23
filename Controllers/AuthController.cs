using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

[ApiController]
[Route("/[controller]")]
public class AuthController : ControllerBase
{
    private readonly InMemoryDBSetContext _dbContext;

    private readonly JwtTokenService _jwtTokenService;
    public AuthController(InMemoryDBSetContext dbContext, JwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] CreateUserDTO createUserDTO)
    {
        var userQuery = _dbContext.Users.FirstOrDefault(u => u.Username == createUserDTO.Username);
        if (userQuery != null)
        {
            return BadRequest(new { msg = "Username already taken." });
        }
        User user = new();
        user.Username = createUserDTO.Username;
        user.Password = PasswordHasher.Hash(createUserDTO.Password);
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;
        _dbContext.Users.Add(
            user
        );
        _dbContext.SaveChanges();
        return Ok(new ReturnUserDTO(user));
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] CreateUserDTO createUserDTO)
    {
        var userQuery = _dbContext.Users.FirstOrDefault(u => u.Username == createUserDTO.Username);
        if (userQuery == null)
        {
            return BadRequest(new { msg = "User doesn't exist." });
        }
        if (!PasswordHasher.Verify(createUserDTO.Password, userQuery.Password))
        {
            return BadRequest(new { msg = "Wrong password." });
        }
        var token = _jwtTokenService.GenerateToken(createUserDTO.Username);
        return Ok(new { token = token });
    }
}