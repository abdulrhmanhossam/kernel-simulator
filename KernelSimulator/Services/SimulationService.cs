using KernelSimulator.Models;
using KernelSimulator.Models.Enums;

namespace KernelSimulator.Services;

public class SimulationService
{
    private Timer? _timer;
    private bool _isRunning = false;
    private int _quantum = 2;
    private int _currentQuantumCounter = 0;
    private readonly List<string> _logs = new();
    private readonly SchedulerService _scheduler;
    private Process? _currentProcess;
    private readonly List<Process> _blockedProcesses = new();
    public int CurrentTick { get; private set; }

    public SimulationService(SchedulerService scheduler)
    {
        _scheduler = scheduler;
    }

    public void AddProcess(Process process)
    {
        _scheduler.EnqueueProcess(process);
    }

    public void Tick()
    {
        CurrentTick++;

        HandleBlockedProcesses();
        ApplyAging();
        SelectProcessIfNeeded();
        ExecuteCurrentProcess();
    }

    public Process? GetCurrentProcess()
    {
        return _currentProcess;
    }

    public IReadOnlyCollection<Process> GetReadyQueue()
    {
        return _scheduler.GetReadyProcesses();
    }

    public IReadOnlyList<string> GetLogs()
    {
        return _logs.AsReadOnly();
    }
    public void StartAuto(int intervalInSeconds = 1)
    {
        if (_isRunning)
            return;

        _timer = new Timer(_ =>
        {
            Tick();
        }, null, 0, intervalInSeconds * 1000);

        _isRunning = true;
        _logs.Add("Auto simulation started.");
    }

    public void StopAuto()
    {
        _timer?.Dispose();
        _timer = null;
        _isRunning = false;

        _logs.Add("Auto simulation stopped.");
    }

    public void Reset()
    {
        StopAuto();
        CurrentTick = 0;
        _currentProcess = null;
        _currentQuantumCounter = 0;
        _logs.Clear();
        _scheduler.Clear();
        _blockedProcesses.Clear();
    }
    private void ExecuteCurrentProcess()
    {
        if (_currentProcess == null)
            return;

        _currentProcess.RemainingTime--;
        _currentQuantumCounter++;

        _logs.Add(
            $"Process {_currentProcess.Name} executed. Remaining time: {_currentProcess.RemainingTime}"
        );

        if (CheckAndHandleIoRequest())
            return;

        if (IsProcessCompleted())
            return;

        if (IsQuantumExpired())
            HandleQuantumExpiration();
    }

    private void HandleBlockedProcesses()
    {
        foreach (var process in _blockedProcesses.ToList())
        {
            process.IoRemainingTime--;

            if (process.IoRemainingTime <= 0)
            {
                process.State = ProcessState.Ready;
                _blockedProcesses.Remove(process);
                _scheduler.EnqueueProcess(process);

                _logs.Add(
                    $"Process {process.Name} finished I/O and moved to Ready."
                );
            }
        }
    }

    private bool CheckAndHandleIoRequest()
    {
        if (_currentProcess!.IoRequestAtTick.HasValue &&
            CurrentTick == _currentProcess.IoRequestAtTick.Value)
        {
            _currentProcess.State = ProcessState.Blocked;
            _currentProcess.IoRemainingTime = _currentProcess.IoBlockingTime;

            _blockedProcesses.Add(_currentProcess);

            _logs.Add(
                $"Process {_currentProcess.Name} requested I/O and moved to Blocked."
            );

            _currentProcess = null;
            _currentQuantumCounter = 0;

            return true;
        }

        return false;
    }

    private void ApplyAging()
    {
        _scheduler.ApplyAgingToWaitingProcesses();
    }

    private void SelectProcessIfNeeded()
    {
        if (_currentProcess != null)
            return;

        _currentProcess = _scheduler.DequeueNextProcess();

        if (_currentProcess == null)
        {
            _logs.Add("CPU is idle.");
            return;
        }

        _currentProcess.State = ProcessState.Running;
        _currentQuantumCounter = 0;
        _logs.Add($"Process {_currentProcess.Name} started running.");
    }

    private bool IsProcessCompleted()
    {
        if (_currentProcess!.RemainingTime > 0)
            return false;

        _currentProcess.State = ProcessState.Finished;
        _logs.Add($"Process {_currentProcess.Name} finished execution.");

        _currentProcess = null;
        _currentQuantumCounter = 0;

        return true;
    }
    private bool IsQuantumExpired()
    {
        return _currentQuantumCounter >= _quantum;
    }

    private void HandleQuantumExpiration()
    {
        _currentProcess!.State = ProcessState.Ready;
        _scheduler.EnqueueProcess(_currentProcess);

        _logs.Add($"Process {_currentProcess.Name} quantum expired, moved back to queue.");

        _currentProcess = null;
        _currentQuantumCounter = 0;
    }

}
