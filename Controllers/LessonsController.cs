using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PolyglotAPI.Data.Models;
using PolyglotAPI.Data.Repos;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ILogger<LessonsController> _logger;

    public LessonsController(ILessonRepository lessonRepository, ILogger<LessonsController> logger)
    {
        _lessonRepository = lessonRepository;
        _logger = logger;
    }

    // GET: api/Lessons
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons()
    {
        _logger.LogInformation("Getting all lessons");
        var lessons = await _lessonRepository.GetAllAsync();
        return Ok(lessons);
    }

    // GET: api/Lessons/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Lesson>> GetLesson(int id)
    {
        _logger.LogInformation($"Getting lesson with ID: {id}");
        var lesson = await _lessonRepository.GetByIdAsync(id);
        if (lesson == null)
        {
            _logger.LogWarning($"Lesson with ID: {id} not found");
            return NotFound();
        }
        return Ok(lesson);
    }

    // POST: api/Lessons
    [HttpPost]
    public async Task<ActionResult<Lesson>> AddLesson(Lesson lesson)
    {
        _logger.LogInformation("Adding a new lesson");
        try
        {
            await _lessonRepository.AddAsync(lesson);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error adding lesson");
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
    }

    // PUT: api/Lessons/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLesson(int id, Lesson lesson)
    {
        if (id != lesson.Id)
        {
            _logger.LogWarning($"Lesson ID mismatch: {id} != {lesson.Id}");
            return BadRequest();
        }
        _logger.LogInformation($"Updating lesson with ID: {id}");
        await _lessonRepository.UpdateAsync(lesson);
        return NoContent();
    }

    // DELETE: api/Lessons/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        _logger.LogInformation($"Deleting lesson with ID: {id}");
        try
        {
            await _lessonRepository.DeleteAsync(id);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error deleting lesson");
            return BadRequest();
        }
        
        return NoContent();
    }
}
