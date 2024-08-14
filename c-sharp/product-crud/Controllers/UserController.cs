using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = new ApiResponse<IEnumerable<User>>
            {
                StatusCode = 200,
                Status = "success",
                Message = "Users retrieved successfully",
                Data = users
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<User>>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<User>
                {
                    StatusCode = 404,
                    Status = "fail",
                    Message = "User not found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<User>
            {
                StatusCode = 200,
                Status = "success",
                Message = "User retrieved successfully",
                Data = user
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> CreateUser(User user)
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, new ApiResponse<User>
            {
                StatusCode = 201,
                Status = "success",
                Message = "User created successfully",
                Data = createdUser
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest(new ApiResponse<User>
                {
                    StatusCode = 400,
                    Status = "fail",
                    Message = "User ID mismatch",
                    Data = null
                });
            }
            await _userService.UpdateUserAsync(user);
            return Ok(new ApiResponse<User>
            {
                StatusCode = 200,
                Status = "success",
                Message = "User updated successfully",
                Data = user
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<User>
                {
                    StatusCode = 404,
                    Status = "fail",
                    Message = "User not found",
                    Data = null
                });
            }
            await _userService.DeleteUserAsync(id);
            return Ok(new ApiResponse<User>
            {
                StatusCode = 200,
                Status = "success",
                Message = "User deleted successfully",
                Data = null
            });
        }
    }

}