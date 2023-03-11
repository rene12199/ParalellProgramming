﻿using Project.PowerBalancer.BaseClasses;

namespace Project.PowerBalancer.Modules.Clocks;

public class TimeClock : BaseClock
{
    private readonly int _sleepTime;

    private bool _isActive = true;

    public TimeClock(int sleepTime)
    {
        _sleepTime = sleepTime;
    }

    public override void Start()
    {
        while (_isActive)
        {
            Thread.Sleep(_sleepTime);
            Tick();
        }
    }
}