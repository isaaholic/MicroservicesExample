﻿using PlatformService.Data.Interfaces;
using PlatformService.Models;

namespace PlatformService.Data.Concretes;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;

    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }

    public async void CreatePlatform(Platform platform)
    {
        if(platform == null) throw new ArgumentNullException(nameof(platform));
        await _context.Platforms.AddAsync(platform);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms.ToList();
    }

    public Platform GetPlatformById(int id)
    {
        return _context.Platforms.FirstOrDefault(p => p.Id == id);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}
