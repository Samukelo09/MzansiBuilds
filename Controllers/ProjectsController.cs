using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MzansiBuilds.Data;
using MzansiBuilds.Models;

namespace MzansiBuilds.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ProjectsController(ApplicationDbContext context) => _context = context;

    // New user
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            return BadRequest("Username already taken.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully! You can now login." });
    }

    // GET: api/projects/feed -> Requirement 3: Live Feed
    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed() =>
        Ok(await _context.Projects.OrderByDescending(p => p.CreatedAt).ToListAsync());

    // GET: api/projects/celebration -> Requirement 5: Celebration Wall
    [HttpGet("celebration")]
    public async Task<IActionResult> GetCelebration() =>
        Ok(await _context.Projects.Where(p => p.IsCompleted).ToListAsync());

    // POST: api/projects -> Requirement 2: New Entry
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        project.OwnerUsername = User.Identity?.Name ?? "Anonymous";
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return Ok(project);
    }

    // POST: api/projects/collab -> Requirement 3: Raise Hand/Comment
    [Authorize]
    [HttpPost("collab")]
    public async Task<IActionResult> RaiseHand([FromBody] CollaborationRequest request)
    {
        // Sets the commenter's name from their login token
        request.FromUser = User.Identity?.Name ?? "Anonymous";

        _context.CollaborationRequests.Add(request);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Collaboration request sent!" });
    }

    // GET: api/projects/search?term=AI
    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> SearchProjects([FromQuery] string term)
    {
        var results = await _context.Projects
            .Where(p => p.Title.Contains(term) || p.Description.Contains(term))
            .ToListAsync();
        return Ok(results);
    }

    // PUT: api/projects/{id}/complete
    [Authorize]
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> MarkAsComplete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();

        // Check if the person logged in owns this project
        if (project.OwnerUsername != User.Identity?.Name) return Forbid();

        project.IsCompleted = true;
        project.Stage = "Live";
        await _context.SaveChangesAsync();
        return Ok(new { message = "Project moved to Celebration Wall!" });
    }

    // DELETE: api/projects/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();
        if (project.OwnerUsername != User.Identity?.Name) return Forbid();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Project deleted successfully" });
    }
}