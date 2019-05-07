using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.VFX;

[RequireComponent(typeof(VisualEffect))]
public class RhythmNode : BaseNode
{
    public int duration = 1;
    private int currentTick = 1;
    private int currentTickGlobal = 0;
    public bool activateOnPlay = false;
    public int activationTick = 999999;
    public bool showDebug = false;

    // Update is called once per frame
    void Update()
    {

    }

    internal override void OnStart()
    {
        base.OnStart();
        DoOnTick = WaitForActivation;
        MusicManager.Instance.DoOnTick(OnTick);

    }

    public override void Activate()
    {
        base.Activate();
        foreach (BaseNode node in childNodes)
        {
            if (node != this && node.GetType() == typeof(RhythmNode))
            {
                RhythmNode rhythmNode = node as RhythmNode;
                rhythmNode.ActivateAtTick(MusicManager.Instance.CurrentTick() + duration);
            }
        }
        DoOnTick = WaitForDeactivation;
        Debug.Log("tick:" + currentTick + transform.name);
    }

    public void ActivateAtTick(int _tick)
    {
        activationTick = _tick;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        currentTick = 1;
        DoOnTick = WaitForActivation;
    }

    public void WaitForDeactivation(int _tick)
    {
        currentTickGlobal = _tick;
        currentTick++;
        if (currentTick > duration)
        {
            Deactivate();
        }
        else
        {
            Debug.Log("tick:" + currentTick + transform.name);

        }

    }

    public void TickUpdate()
    {
    }
    public virtual void WaitForActivation(int _tick)
    {
        if (showDebug) Debug.Log(_tick);
        currentTickGlobal = _tick;
        if(_tick == activationTick)
        {
            Activate();
        }
    }
}
