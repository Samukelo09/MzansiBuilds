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
}