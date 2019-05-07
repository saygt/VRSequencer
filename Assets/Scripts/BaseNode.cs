using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseNode : MonoBehaviour
{
    public BaseNode parentTarget;

    public List<BaseNode> childNodes;
    public Action<int> DoOnTick = delegate { };
    public Action<int> DoBeforeTick = delegate { };


    public bool canGrab = true;
    public AudioHelm.Sampler sampler;
    public AudioHelm.HelmController helm;
    public ArcController arcPrefab;
    private ArcController rayInstance;
    public TriggerSensor connectorSensor;
    public TriggerSensor grabSensor;
    private Material mat;
    private Light m_light;
    private Rigidbody rb;
    public Color defaultColor;
    public Color hoverColor;
    public Color activeColor;
    public float lightIntensity = 0.5f;
    public enum GrabbableState { Idle, Hovering, Grabbing }
    public GrabbableState grabState = GrabbableState.Idle;

    public enum NodeTypes { Rhythm, Instrument, Chord}
    public NodeTypes nodeType;
    private Type connectType; 

    private AudioReactor currentParent;
    internal bool isActivated;

    // Use this for initialization
    private void Start()
    {
        OnStart();
        if (connectorSensor)
        {
            //connectorSensor.onTriggerEnter = OnReactorHover;
            //connectorSensor.onTriggerExit = OnReactorExit;
        }
        switch ( nodeType )
        {
            case NodeTypes.Rhythm:
                connectType = typeof(RhythmNode);
                break;
            case NodeTypes.Instrument:
                connectType = typeof(AudioReactor);
                break;
            default:
                connectType = typeof(BaseNode);
                break;
        }
        if(parentTarget)
        {
            ConnectToNode(parentTarget);
        }
    }

    internal virtual void OnStart()
    {
    }

    private void Update()
    {
        if (grabState == GrabbableState.Grabbing)
        {
            if (connectorSensor.GetCount(connectType) > 0)
            {
                BaseNode _parentTarget = connectorSensor.GetClosest(transform.position, typeof(RhythmNode)).GetComponentInParent<BaseNode>();
                if (parentTarget == _parentTarget || _parentTarget.parentTarget == this) return;
                ConnectToNode(_parentTarget);
            }
            else
            {
                if(parentTarget) DisconnectFromNode();
                if (rayInstance)
                {
                    DisconnectFromNode();
                }
            }
        }

        // check for required ray instance
    }

    public virtual void ConnectToNode(BaseNode other)
    {
        // if no instance of ray, instantiate new ray object

        if(parentTarget)
        {
            parentTarget.RemoveChild(this);
        }
        InstantiateRay(other);
        other.AddChild(this);
        parentTarget = other;
    }

    public virtual void DisconnectFromNode()
    {
        if(parentTarget)
        {
            parentTarget.RemoveChild(this);
            parentTarget = null;
        }
        if(rayInstance)
        {
            Destroy(rayInstance.gameObject);
        }
    }

    public virtual BaseNode Duplicate()
    {
        return null;
    }

    public virtual void InstantiateRay( BaseNode target )
    {
        if (!rayInstance)
        {
            rayInstance = Instantiate(arcPrefab);
        }
        rayInstance.SetOwner(transform);
        rayInstance.SetTarget(target.transform);
    }

    public void AddChild( BaseNode _node)
    {
        childNodes.Add(_node);
    }

    public void RemoveChild( BaseNode _node)
    {
        childNodes.Remove(_node);
    }

    public virtual void OnReactorExit(GameObject other)
    {

    }

    public virtual void Select()
    {

    }

    public virtual void Deselect()
    {

    }

    public virtual void Grab()
    {
        grabState = GrabbableState.Grabbing;
    }

    public virtual void Release()
    {
        grabState = GrabbableState.Idle;
    }
  

    public virtual void Activate()
    {
        isActivated = true;
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
        isActivated = false;
    }




}
