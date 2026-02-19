using Microsoft.AspNetCore.Mvc;
using QuestionBankService.Models;
 
using Microsoft.EntityFrameworkCore;
using QuestionBankService.Data;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly QuestionDbContext _context;

    public QuestionsController(QuestionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Questions.ToListAsync());
    }
    [HttpGet("subject/{subject}")]
public async Task<IActionResult> GetBySubject(string subject)
{
    var questions = await _context.Questions
        .Where(q => q.Subject == subject && q.IsActive)
        .ToListAsync();

    return Ok(questions);
}
[HttpGet("difficulty/{difficulty}")]
public async Task<IActionResult> GetByDifficulty(string difficulty)
{
    var questions = await _context.Questions
        .Where(q => q.Difficulty == difficulty && q.IsActive)
        .ToListAsync();

    return Ok(questions);
}
[HttpGet("generate")]
public async Task<IActionResult> GenerateQuestionPaper(
    string subject,
    string difficulty,
    int count)
{
    var questions = await _context.Questions
        .Where(q =>
            q.Subject == subject &&
            q.Difficulty == difficulty &&
            q.IsActive)
        .OrderBy(q => Guid.NewGuid()) // RANDOM
        .Take(count)
        .ToListAsync();

    if (!questions.Any())
        return NotFound("No questions found for given criteria");

    return Ok(new
    {
        PaperId = Guid.NewGuid(),
        Subject = subject,
        Difficulty = difficulty,
        TotalQuestions = questions.Count,
        Questions = questions
    });
}
[HttpPost("generate-paper")]
public async Task<IActionResult> GeneratePaper([FromBody] GeneratePaperRequest request)
{
    if (request == null)
        return BadRequest("Request is null");

    if (string.IsNullOrWhiteSpace(request.Subject))
        return BadRequest("Subject missing");

    if (request.EasyCount < 0 || request.MediumCount < 0 || request.HardCount < 0)
        return BadRequest("Counts invalid");

    var subject = request.Subject.Trim().ToLower();

    var easy = await _context.Questions
        .Where(q => q.Subject.ToLower() == subject &&
                    q.Difficulty.ToLower() == "easy" &&
                    q.IsActive)
        .OrderBy(q => Guid.NewGuid())
        .Take(request.EasyCount)
        .ToListAsync();

    var medium = await _context.Questions
        .Where(q => q.Subject.ToLower() == subject &&
                    q.Difficulty.ToLower() == "medium" &&
                    q.IsActive)
        .OrderBy(q => Guid.NewGuid())
        .Take(request.MediumCount)
        .ToListAsync();

    var hard = await _context.Questions
        .Where(q => q.Subject.ToLower() == subject &&
                    q.Difficulty.ToLower() == "hard" &&
                    q.IsActive)
        .OrderBy(q => Guid.NewGuid())
        .Take(request.HardCount)
        .ToListAsync();

    return Ok(new
    {
        request,
        easyCount = easy.Count,
        mediumCount = medium.Count,
        hardCount = hard.Count,
        questions = easy.Concat(medium).Concat(hard)
    });
}
[HttpGet("subjects")]
public async Task<IActionResult> GetSubjects()
{
    var subjects = await _context.Questions
        .Where(q => q.IsActive)
        .Select(q => q.Subject)
        .Distinct()
        .OrderBy(s => s)
        .ToListAsync();

    return Ok(subjects);
}

}