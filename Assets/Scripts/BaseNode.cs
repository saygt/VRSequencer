using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseNode : MonoBehaviour
{
    public List<BaseNode> childNodes;
    public Action<int> DoOnTick = delegate { };
    public Action<int> DoBeforeTick = delegate { };

    // Use this for initialization
    private void Start()
    {
        OnStart();
    }

    internal virtual void OnStart()
    {
    }

    public virtual void Activate()
    {

    }

    public void OnTick(int _tick)
    {
        DoOnTick(_tick);
    }

    public void BeforeTick(int _tick)
    {
        DoBeforeTick(_tick);
    }

    public virtual void Deactivate()
    {

    }




}
