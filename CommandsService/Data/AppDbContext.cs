﻿using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Command> Commands { get; set; }
}
