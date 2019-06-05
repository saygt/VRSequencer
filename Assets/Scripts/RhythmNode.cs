using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.VFX;
using DG.Tweening;

public class RhythmNode : BaseNode
{
    public int duration = 1;
    private int currentTick = 1;
    private int currentTickGlobal = 0;
    public bool activateOnPlay = false;
    public int activationTick = 999999;
    public bool showDebug = false;
    public List<AudioReactor> audioNodes = new List<AudioReactor>();
    private List<InstrumentNode> activeInstruments = new List<InstrumentNode>();

    internal override void OnStart()
    {
        base.OnStart();
        DoOnTick = WaitForActivation;
        MusicManager.Instance.DoOnTick(OnTick);
        vfx = GetComponent<VisualEffect>();
        if(!vfx)
        {
            vfx = GetComponentInChildren<VisualEffect>();
        }
    }

    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    public override void Activate()
    {
        base.Activate();
        foreach (BaseNode _node in childNodes)
        {
            if (_node != this && _node.GetType() == typeof(RhythmNode))
            {
                RhythmNode rhythmNode = _node as RhythmNode;
                rhythmNode.ActivateAtTick(MusicManager.Instance.CurrentTick() + duration);
            }
            if(_node != this && _node.GetType() == typeof(InstrumentNode))
            {
                _node.Activate(duration * MusicManager.Instance.TimePerTick());
                activeInstruments.Add(_node as InstrumentNode);
            }
        }

        DoOnTick = WaitForDeactivation;
        float durationInSeconds = MusicManager.Instance.TimePerTick()*duration;
        float pulseDuration = 0.2f;
        vfx.SetFloat("scale", 0.2F);
        DOTween.Sequence()
            .Append(DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), 0.9f, pulseDuration).SetEase(Ease.OutCubic))
            .Append(DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), 0.2f, durationInSeconds - pulseDuration).SetEase(Ease.InCubic))
            ;
    }

    public override BaseNode Duplicate()
    {

        RhythmNode _node = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<RhythmNode>();
        _node.rayInstance = null;
        _node.childNodes.Clear();
        _node.activationTick = 9999999;
        _node.Deactivate();
        _node.parentTarget = this;
        return _node;
    }

    public void ActivateAtTick(int _tick)
    {
        activationTick = _tick;
        isActivated = true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        foreach (InstrumentNode _node in activeInstruments)
        {
            _node.Deactivate();
        }
        activeInstruments.Clear();
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

        }

    }

    public void TickUpdate()
    {
    }
    public virtual void WaitForActivation(int _tick)
    {
        if (showDebug) Debug.Log(_tick);
        currentTickGlobal = _tick;

        if( _tick == activationTick )
        {
            Activate();
            return;
        }

        // if the node has no parents, check for active child nodes and if none exists, activate itself
        if (parentTarget == null && childNodes.Count > 0 )
        {
            BaseNode activeChild = GetFirstActiveChild(this);
            if( activeChild == null )
            {
                Activate();
            }
        }

    }

    public BaseNode GetFirstActiveChild( BaseNode _node)
    {
        foreach( BaseNode child in _node.childNodes)
        {
            if (child.isActivated)
            {
                return child;
            }
            else
            {
                BaseNode activeGrandchild = GetFirstActiveChild(child);
                if(activeGrandchild != null)
                {
                    return activeGrandchild;
                }
            }
        }
        return null;
    }
}
