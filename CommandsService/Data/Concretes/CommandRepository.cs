using CommandsService.Data.Interfaces;
using CommandsService.Models;

namespace CommandsService.Data.Concretes;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        command.PlatformId = platformId;

        _context.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform == null) throw new ArgumentNullException(nameof(platform));

        _context.Platforms.Add(platform);
    }

    public IEnumerable<Platform> GetAllPlatforms()
        => _context.Platforms.ToList();

    public Command GetCommand(int platformId, int commandId)
        => _context.Commands
        .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        => _context.Commands
        .Where(c => c.PlatformId == platformId)
        .OrderBy(c => c.Platform.Name);

    public bool PlatformExist(int id)
        => _context.Platforms.Any(p => p.Id == id);

    public bool SaveChanges()
        => _context.SaveChanges() >= 0;
}
