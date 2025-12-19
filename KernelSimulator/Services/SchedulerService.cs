using KernelSimulator.Models;
using KernelSimulator.Models.Enums;

namespace KernelSimulator.Services;

public class SchedulerService
{
    private readonly Queue<Process> _readyQueue = new();

    public void EnqueueProcess(Process process)
    {
        if (process.State == ProcessState.Ready)
            _readyQueue.Enqueue(process);
    }

    public Process? DequeueNextProcess()
    {
        if (_readyQueue.Any())
            return _readyQueue.Dequeue();

        return null;
    }
    public IReadOnlyCollection<Process> GetReadyProcesses()
    {
        return _readyQueue.ToList().AsReadOnly();
    }

    public int ReadyCount() => _readyQueue.Count;

    public void Clear() => _readyQueue.Clear();
}
