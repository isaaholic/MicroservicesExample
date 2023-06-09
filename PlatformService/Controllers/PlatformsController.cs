﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Interfaces;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(IPlatformRepository repository, IMapper mapper, ICommandDataClient commandDataClient)
    {
        _repository = repository;
        _mapper = mapper;
        this._commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting Platforms...");

        var platforItems = _repository.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforItems));
    }

    [HttpGet("{id}",Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var platforItem = _repository.GetPlatformById(id);
        if(platforItem is not null)
        {
            return Ok(_mapper.Map<PlatformReadDto>(platforItem));
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platformModel = _mapper.Map<Platform>(platformCreateDto);
        _repository.CreatePlatform(platformModel);
        _repository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            throw;
        }

        return CreatedAtRoute(nameof(GetPlatformById), new { id = platformModel.Id }, platformReadDto);
    }
}
