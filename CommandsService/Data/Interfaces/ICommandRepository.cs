﻿using CommandsService.Models;

namespace CommandsService.Data.Interfaces;

public interface ICommandRepository
{
    bool SaveChanges();

    // Platforms
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExist(int id);

    // Commands
    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command GetCommand(int platformId,int commandId);
    void CreateCommand(int platformId, Command command);
    
}
