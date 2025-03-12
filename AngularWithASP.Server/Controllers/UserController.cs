using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private static List<string> users = new List<string> { "Alice", "Bob", "Charlie" };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }

    [HttpPost]
    public IActionResult AddUser([FromBody] string user)
    {
        users.Add(user);
        return Ok(users);
    }
}
