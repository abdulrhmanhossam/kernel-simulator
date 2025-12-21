using KernelSimulator.Dtos;
using KernelSimulator.Models;

namespace KernelSimulator.Factories;

public static class ProcessFactory
{
    public static Process Create(CreateProcessRequestDto requestDto)
    {
        return new Process(requestDto.Id, requestDto.Name, requestDto.BurstTime,
            requestDto.Priority, requestDto.IoRequestAtTick, requestDto.IoBlockingTime);
    }
}
