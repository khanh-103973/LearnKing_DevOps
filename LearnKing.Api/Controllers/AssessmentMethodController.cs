using Microsoft.AspNetCore.Mvc;

namespace LearnKing.Api.Controllers
{
    [ApiController]
    [Route("api/assessment-method")]
    public class AssessmentMethodController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new[]
            {
                new { Id = 1, Name = "Quiz", Description = "Multiple Choice Quiz" },
                new { Id = 2, Name = "Assignment", Description = "Practical Code Submission" }
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] object data)
        {
            return Ok(new { Message = "Assessment method created successfully", Data = data });
        }

        [HttpPut]
        public IActionResult Update([FromBody] object data)
        {
            return Ok(new { Message = "Assessment method updated successfully", Data = data });
        }

        [HttpGet("paging")]
        public IActionResult GetPaging([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 2,
                Items = new[]
                {
                    new { Id = 1, Name = "Quiz" },
                    new { Id = 2, Name = "Assignment" }
                }
            });
        }

        [HttpGet("public/paging")]
        public IActionResult GetPublicPaging([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(new
            {
                IsPublic = true,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 2,
                Items = new[]
                {
                    new { Id = 1, Name = "Public Quiz" },
                    new { Id = 2, Name = "Public Assignment" }
                }
            });
        }
    }
}
