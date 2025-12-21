using KernelSimulator.Dtos;
using KernelSimulator.Factories;
using KernelSimulator.Services;
using Microsoft.AspNetCore.Mvc;

namespace KernelSimulator.Controllers;

[Route("api/simulation")]
[ApiController]
public class SimulationController : ControllerBase
{
    private readonly SimulationService _simulation;

    public SimulationController(SimulationService simulation)
    {
        _simulation = simulation;
    }

    [HttpPost("process")]
    public IActionResult CreateProcess([FromBody] CreateProcessRequestDto requestDto)
    {
        var process = ProcessFactory.Create(requestDto);
        _simulation.AddProcess(process);

        return Ok(new
        {
            message = "Process created successfully",
            process
        });
    }

    [HttpPost("tick")]
    public IActionResult Tick()
    {
        _simulation.Tick();

        return Ok(new
        {
            tick = _simulation.CurrentTick,
            runningProcess = _simulation.GetCurrentProcess()
        });
    }

    [HttpGet("queue")]
    public IActionResult ReadyQueue()
    {
        return Ok(_simulation.GetReadyQueue());
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        return Ok(new
        {
            tick = _simulation.CurrentTick,
            runningProcess = _simulation.GetCurrentProcess()
        });
    }

    [HttpGet("logs")]
    public IActionResult Logs()
    {
        return Ok(_simulation.GetLogs());
    }

    [HttpPost("start")]
    public IActionResult StartAuto()
    {
        _simulation.StartAuto();
        return Ok(new { message = "Auto simulation started" });
    }

    [HttpPost("stop")]
    public IActionResult StopAuto()
    {
        _simulation.StopAuto();
        return Ok(new { message = "Auto simulation stopped" });
    }

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _simulation.Reset();
        return Ok(new { message = "Simulation reset successfully" });
    }
}
