# Kernel Simulator (Educational OS Project)

This project is a **simplified kernel simulator** built with ASP.NET Core.
It demonstrates core **Operating System concepts** in a clean, readable,
and incremental way.

## 🚀 Concepts Implemented

- Process Scheduling
- Round Robin Scheduling
- Priority Scheduling
- Preemptive Priority
- Process States (Ready, Running, Blocked, Finished)
- I/O Blocking Simulation
- Aging (Starvation Prevention)
- Time Quantum
- Tick-based CPU Simulation

---

## 🧠 Architecture Overview

The system is driven by a **Tick-based simulation loop**.

Each Tick represents one unit of CPU time and performs the following steps:

1. Handle blocked processes (I/O)
2. Apply aging to waiting processes
3. Select a process if the CPU is idle
4. Preempt the running process if a higher-priority one exists
5. Execute the current process

---

## 🧩 Main Components

### SimulationService
Responsible for:
- Tick orchestration
- CPU execution
- Quantum handling
- Preemption
- I/O blocking

### SchedulerService
Responsible for:
- Managing the Ready Queue
- Selecting the next process
- Applying aging

### Process
Represents a simulated OS process including:
- Execution time
- Priority
- I/O behavior
- State transitions

---

## 4️⃣ Test Scenarios (بديل

### 🔹 Scenario 1: CPU Idle
- Run Tick
- Expected: CPU is idle

---

### 🔹 Scenario 2: Round Robin
- Process A (Burst 5)
- Quantum = 2
- Expected:
  - Executes
  - Returns to queue
  - Executes again

---

### 🔹 Scenario 3: I/O Blocking
- Process with `IoRequestAtTick = 2`
- Expected:
  - Moves to Blocked
  - Returns after I/O time

---

### 🔹 Scenario 4: Preemption
- Running process priority = 5
- New process priority = 1
- Expected:
  - Running process preempted
  - Higher priority process executes
