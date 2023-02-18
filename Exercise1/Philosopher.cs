﻿using System.Security.Cryptography;

namespace Exerise1;

//todo: use concurrency primitives correctly

public class Philosopher
{
    private readonly Restaurant _restaurant;

    private static int n;

    public bool IsOpen { get; set; } = true;

    public string Name { get; set; }

    public int Index { get; set; }

    public Philosopher(Restaurant restaurant)
    {
        _restaurant = restaurant;
        Index = n;
        Name = $"Philosopher {n++}";
    }

    public void Start(int thinkingTime, int eatingTime)
    {
        while (IsOpen)
        {
            Console.WriteLine($"{Name} is thinking");
            Thread.Sleep(RandomNumberGenerator.GetInt32(thinkingTime * 1000));
            Console.WriteLine($"{Name} finished thinking");
            Fork fork1;
            Fork fork2;
            if (Index % 2 == 0)
            {
                fork1 = _restaurant.TakeFork(Index);
                fork2 = _restaurant.TakeFork(Index + 1);
            }
            else
            {
                fork1 = _restaurant.TakeFork(Index + 1);
                fork2 = _restaurant.TakeFork(Index);
            }

            Thread.Sleep(100);
            Monitor.Enter(fork2);

            fork2.UseFork(this);

            Thread.Sleep(1000);
            Monitor.Enter(fork1);

            fork1.UseFork(this);

            Monitor.PulseAll(fork2);
            Monitor.PulseAll(fork1);
            Thread.Sleep(RandomNumberGenerator.GetInt32(eatingTime * 1000));
            _restaurant.TakeFork(Index + 1).PutDownFork();
            _restaurant.TakeFork(Index).PutDownFork();
        }
    }
}