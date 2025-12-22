using KernelSimulator.Models;

namespace KernelSimulator.Services;

public class SchedulerService
{
    private readonly List<Process> _readyProcesses = new();

    public void EnqueueProcess(Process process)
    {
        _readyProcesses.Add(process);
    }

    public Process? DequeueNextProcess()
    {
        if (!_readyProcesses.Any())
            return null;

        var nextProcess = _readyProcesses
            .OrderBy(p => p.Priority)
            .First();

        _readyProcesses.Remove(nextProcess);
        return nextProcess;
    }

    public void ApplyAgingToWaitingProcesses()
    {
        foreach (var process in _readyProcesses)
            process.ApplyAging();
    }

    public IReadOnlyCollection<Process> GetReadyProcesses()
    {
        return _readyProcesses.AsReadOnly();
    }

    public int ReadyCount() => _readyProcesses.Count;

    public void Clear() => _readyProcesses.Clear();
}
