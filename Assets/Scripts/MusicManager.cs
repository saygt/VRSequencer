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

    public Action<int> onUpdate = i => { };

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
        currentTick = Mathf.FloorToInt(currentTime / (timePerTick));
        if(currentTick != lastTick)
        {
            onUpdate(currentTick);
            lastTick = currentTick;
        }
    }

    public void AddUpdateAction( Action<int> _action )
    {
        onUpdate += _action;
    }
}
