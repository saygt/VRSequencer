using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.VFX;

public class BaseNode : MonoBehaviour
{
    public NodeTypes nodeType;
    public BaseNode parentTarget;
    public List<BaseNode> childNodes;
    public VisualEffect vfx;
    public TriggerSensor connectorSensor;
    public TriggerSensor grabSensor;
    public AudioHelm.Sampler sampler;
    public AudioHelm.HelmController helm;
    public ArcController arcPrefab;

    public Color defaultColor;
    public Color hoverColor;
    public Color activeColor;
    public float lightIntensity = 0.5f;
    public float vfxScale = 1f;

    public bool canGrab = true;
    public GrabbableState grabState = GrabbableState.Idle;

    public Action<int> DoOnTick = delegate { };
    public Action<int> DoBeforeTick = delegate { };

    private ArcController rayInstance;
    private Material mat;
    private Light m_light;
    private Rigidbody rb;
    public enum GrabbableState { Idle, Hovering, Grabbing }
    public enum NodeTypes { Rhythm, Instrument, Chord}
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
        OnUpdate();
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

    internal virtual void OnUpdate()
    {

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
        rayInstance.vfx.Simulate(0.1f, 50);
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

    public virtual void NextOption()
    {

    }

    public virtual void PrevOption()
    {

    }

    public virtual void Select()
    {
        vfx.SetInt("Mode", 1);
    }

    public virtual void Deselect()
    {
        vfx.SetInt("Mode", 0);
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
        vfx.SetInt("Mode", 2);

    }

    public virtual void Activate(float _duration)
    {
        Activate();
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
        vfx.SetInt("Mode", 0);

    }




}
