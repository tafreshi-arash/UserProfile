using Microsoft.AspNetCore.Mvc;

namespace UserProfileController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private IUserProfileManagement userController;

        public UserController(IUserProfileManagement userProfileManagement)
        {
            this.userController = userProfileManagement;
        }

        [HttpGet, Route("Get")]
        // Search By Name or Family or Both
        public async Task<IActionResult> GetUser([FromQuery] string? name, [FromQuery] string? family)
        {
            var result = await userController.GetUserAsync(name, family);
            return Ok(result);
        }
    }
}
