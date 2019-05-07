using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public int bpm = 80;
    private int currentTick = 0;
    private int lastTick = -1;
    private float currentTime;
    private float timePerBeat = 60 / 80;
    private float timePerTick;
    public int ticksPerBeat = 4;

    public Action<int> onTick = i => { };
    public Action<int> beforeTick = i => { };

    // Start is called before the first frame update
    void Start()
    {
        timePerBeat = 60.0f/bpm;
        timePerTick = timePerBeat / ticksPerBeat;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        float currentTickTime = currentTime / timePerTick;
        if(currentTickTime > lastTick+0.995f)
        {
            currentTick = Mathf.RoundToInt( currentTickTime);
            beforeTick(currentTick);
            onTick(currentTick);
            lastTick = currentTick;
        }
    }

    public void DoOnTick( Action<int> _action )
    {
        onTick += _action;
    }
    public void DoBeforeTick( Action<int> _action )
    {
        beforeTick += _action;
    }

    public int CurrentTick()
    {
        return currentTick;
    }
}
