using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyglotAPI.Data.Models;
using PolyglotAPI.Data.Repos;


namespace PolyglotAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseRepository courseRepository, ILogger<CoursesController> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            _logger.LogInformation("Getting all courses");
            var courses = await _courseRepository.GetAllAsync();
            return Ok(courses);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            _logger.LogInformation($"Getting course with ID: {id}");
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                _logger.LogWarning($"Course with ID: {id} not found");
                return NotFound();
            }
            return Ok(course);
        }

        // POST: api/Courses
        [HttpPost]
        public ActionResult<Course> AddCourse(Course course)
        {
            _logger.LogInformation("Adding a new course");
            try
            {
                _courseRepository.AddAsync(course);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error adding course: {e.Message}");
                return BadRequest();
            }
            
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                _logger.LogWarning($"Course ID mismatch: {id} != {course.Id}");
                return BadRequest();
            }
            _logger.LogInformation($"Updating course with ID: {id}");
            _courseRepository.UpdateAsync(course);
            return NoContent();
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            _logger.LogInformation($"Deleting course with ID: {id}");
            _courseRepository.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/Courses/3/Modules
        [HttpGet("{id}/Modules")]
        public async Task<ActionResult<IEnumerable<Module>>> GetModulesForCourse(int id)
        {
            _logger.LogInformation($"Getting all modules for course with ID: {id}");
            var modules = await _courseRepository.GetModulesByCourseIdAsync(id);
            return Ok(modules);
        }
    }
}
