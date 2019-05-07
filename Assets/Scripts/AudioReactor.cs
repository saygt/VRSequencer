using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using DG.Tweening;

public class AudioReactor : MonoBehaviour
{
    public bool canGrab = true;
    public AudioHelm.Sampler sampler;
    public AudioHelm.HelmController helm;
    public ArcController arcPrefab;
    private ArcController arcInstance;
    public TriggerSensor connectorSensor;
    public TriggerSensor reactorSensor;
    private Material mat;
    private Light m_light;
    private Rigidbody rb;
    public Color defaultColor;
    public Color poweredColor;
    public Color playColor;
    public float lightIntensity = 0.5f;
    public int note = 60;
    public enum GrabbableState { Idle, Hovering, Grabbing }
    public GrabbableState grabState = GrabbableState.Idle;
    public float delay = 0f;
    public List<AudioReactor> children = new List<AudioReactor>();

    private AudioReactor parentTarget;
    private AudioReactor currentParent;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInChildren<Renderer>().material;
        m_light = GetComponent<Light>();
        rb = GetComponent<Rigidbody>();
        mat.color = defaultColor;
        if (GetComponent<VisualEffect>())
        {
            GetComponent<VisualEffect>().enabled = false;
            GetComponent<VisualEffect>().enabled = true;
        }
        if (connectorSensor)
        {
            connectorSensor.onTriggerEnter = OnReactorHover;
            connectorSensor.onTriggerExit = OnReactorExit;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(grabState == GrabbableState.Grabbing)
        {
            if (rb)
            {
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            rb.isKinematic = true;

        }
    }

    public void Hover()
    {
        grabState = GrabbableState.Hovering;
    }

    public void Grab()
    {
        grabState = GrabbableState.Grabbing;
        if (currentParent) currentParent.RemoveChild(this);
        Activate(false);

    }

    public void Release()
    {
        grabState = GrabbableState.Idle;
        GameObject _parentTarget = connectorSensor.GetClosest(transform.position, typeof(AudioReactor));
        transform.SetParent(_parentTarget.transform);

        // if currently parented, remove self from parent
        if(currentParent != null)
        {
            currentParent.RemoveChild(this);
        }

        // if there is a target parent, add self to parent
        if(_parentTarget)
        {
            currentParent = _parentTarget.GetComponentInParent<AudioReactor>();
            Debug.Log(_parentTarget.name);
            currentParent.AddChild(this);
        }
    }

    public void AddChild(AudioReactor target)
    {
        children.Add(target);
        target.transform.SetParent(transform);
    }

    public void RemoveChild(AudioReactor target)
    {
        children.Remove(target);
        target.transform.SetParent(null);
    }

    public void OnReactorHover(GameObject other)
    {
        Debug.Log("hover");
        arcInstance = Instantiate(arcPrefab, transform, false);
        arcInstance.SetOwner(transform);
        arcInstance.SetTarget(other.transform);
        AudioReactor reactor = other.GetComponentInParent<AudioReactor>();
        if (reactor && grabState == GrabbableState.Grabbing)
        {
            parentTarget = reactor;

        }
    }

    public void OnReactorExit(GameObject other)
    {
        //parentTarget = null;
        Debug.Log(connectorSensor.GetCount());
        if(connectorSensor.GetCount() == 0)
        {
            Destroy(arcInstance.gameObject);
        }
        //AudioReactor reactor = other.GetComponentInParent<AudioReactor>();
        //if (reactor && grabState == GrabbableState.Grabbing)
        //{
        //    if(parentTarget == other)
        //    {
        //        parentTarget = null;
        //    }
        //}
    }

    public void Activate(bool _on)
    {
        if (_on)
        {
            if (helm)
            {
                
                helm.NoteOn(note);
            }
            if (sampler)
            {
                DOVirtual.DelayedCall(delay, () => sampler.NoteOn(note));
                
            }
            AudioReactor[] children = GetComponentsInChildren<AudioReactor>();
            foreach (AudioReactor child in GetComponentsInChildren<AudioReactor>())
            {
                // prevent infinite recursion
                if (child != this)
                {
                    child.Activate(true);
                }
            }
            mat.color = playColor;
            ChangeColor(playColor, "_EmissionColor", 0.1f);
           // m_light.DOIntensity(lightIntensity, 0.2f);

        }
        else
        {
            if (helm)
            {
                helm.NoteOff(note);
            }
            if (sampler)
            {
                //sampler.NoteOff();
            }
            foreach (AudioReactor child in GetComponentsInChildren<AudioReactor>())
            {
                // prevent infinite recursion
                if (child != this)
                {
                    child.Activate(false);
                }
            }
            if (mat)
            {
                mat.color = defaultColor;
                ChangeColor(defaultColor, "_EmissionColor", 0.1f);

            }
           // m_light.DOIntensity(0, 0.2f);
        }
    }

    public void ChangeColor(Color _color, string _property, float _duration)
    {
        mat.DOColor(_color, _property, _duration);
    }
}
