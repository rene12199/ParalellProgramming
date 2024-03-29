﻿using System.Diagnostics;

namespace Exerise1;

public class Restaurant
{
    private readonly int _thinkingTime;
    private readonly int _eatingTime;
    private bool IsOpen;

    private readonly IList<Philosopher> _philosophers = new List<Philosopher>();
    private readonly IList<Fork> _forks = new List<Fork>();
    public readonly Stopwatch timer = new();


    public Restaurant(int n, int thinkingTime, int eatingTime)
    {
        _thinkingTime = thinkingTime;
        _eatingTime = eatingTime;
        for (int i = 0; i < n; i++) _philosophers.Add(new Philosopher(this));

        for (int i = 0; i < n; i++) _forks.Add(new Fork());
    }

    public Fork TakeFork(int index)
    {
        if (index == _forks.Count)
            index = 0;

        return _forks[index];
    }

    public void Start()
    {
        timer.Start();
        var threads = new List<Thread>();
        foreach (var philosopher in _philosophers)
        {
            threads.Add(new Thread(() => philosopher.Start(_thinkingTime, _eatingTime)));
            threads.Last().Start();
        }

        Console.WriteLine("Press any key to exit");
        Console.ReadLine();
        double waitingTime = 0;
        foreach (var philosopher in _philosophers)
        {
            waitingTime += philosopher.timer.Elapsed.TotalSeconds;
            philosopher.IsOpen = false;
        }

        foreach (var thread in threads) thread.Join();

        timer.Stop();
        Console.WriteLine("Total elapsed time: " + timer.Elapsed.TotalSeconds);
        Console.WriteLine("Total waited time: " + waitingTime);
    }
}