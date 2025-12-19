using KernelSimulator.Models.Enums;

namespace KernelSimulator.Models;

public class Process
{
    public Process(int id, string name, int burstTime)
    {
        Id = id;
        Name = name;
        BurstTime = burstTime;
        RemainingTime = burstTime;
        State = ProcessState.Ready;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    // all time Process will need
    public int BurstTime { get; set; }

    public int RemainingTime { get; set; }

    public ProcessState State { get; set; }
}
