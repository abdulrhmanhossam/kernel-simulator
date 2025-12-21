using KernelSimulator.Models.Enums;

namespace KernelSimulator.Models;

public class Process
{
    public Process(int id, string name, int burstTime, int priority, int? ioRequestAtTick, int ioBlockingTime)
    {
        Id = id;
        Name = name;
        BurstTime = burstTime;
        RemainingTime = burstTime;
        Priority = priority;
        State = ProcessState.Ready;
        IoBlockingTime = ioBlockingTime;
        IoRequestAtTick = ioRequestAtTick;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    // all time Process will need
    public int BurstTime { get; set; }
    public int RemainingTime { get; set; }
    public int Priority { get; set; }
    public ProcessState State { get; set; }
    public int? IoRequestAtTick { get; set; }
    public int IoBlockingTime { get; set; }
    public int IoRemainingTime { get; set; }

}
