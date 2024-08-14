
using Dtos;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<CommentDto> GetComments()
        {
            return
            [
                new() { Id = 1, Description = "Comment 1" , Post = "Post 1", User = "User 1"},
                new() { Id = 2, Description = "Comment 2",  Post = "Post 2", User = "User 2" },
            ];

        }
    }

}
