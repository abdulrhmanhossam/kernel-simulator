namespace KernelSimulator.Dtos;

public class CreateProcessRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BurstTime { get; set; }
    public int Priority { get; set; }
    public int? IoRequestAtTick { get; set; }
    public int IoBlockingTime { get; set; }
}
