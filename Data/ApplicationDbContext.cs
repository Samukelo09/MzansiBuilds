using Microsoft.EntityFrameworkCore;
using MzansiBuilds.Models;
using MzansiBuilds.Data;
using System.Collections.Generic;

namespace MzansiBuilds.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<CollaborationRequest> CollaborationRequests { get; set; }
}