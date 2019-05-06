using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerManager : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean copyAction;
    public Action onUpdate = () => {};
    private GameObject objectInHand; // 2
    private AudioReactor hoveringReactor;
    private AudioReactor grabbedReactor;
    private Vector3 grabOffsetPosition;
    private Quaternion grabOffsetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1
        if (grabAction.GetLastStateDown(handType))
        {
            if(hoveringReactor)
            {
                Grab(hoveringReactor);
            }
        }

        if (copyAction.GetLastStateDown(handType))
        {
            if(hoveringReactor)
            {
                AudioReactor newReactor = Instantiate(hoveringReactor.gameObject, hoveringReactor.transform.position, hoveringReactor.transform.rotation).GetComponent<AudioReactor>();
                Grab(newReactor);
            }
        }

        if (copyAction.GetLastStateUp(handType))
        {

                Release(grabbedReactor.gameObject);

        }

        // 2
        if (grabAction.GetLastStateUp(handType))
        {
            Release(grabbedReactor.gameObject);
        }
        onUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioReactor reactor = other.GetComponentInParent<AudioReactor>();
        if(reactor)
        {
            if (!reactor.canGrab) return;
            hoveringReactor = reactor;
           // reactor.PlayNote(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AudioReactor reactor = other.GetComponentInParent<AudioReactor>();
        if (reactor)
        {
            if (!reactor.canGrab) return;
            hoveringReactor = null;
           // reactor.PlayNote(false);
        }
    }

    public void Grab(AudioReactor target)
    {
        grabbedReactor = target;
        if(grabbedReactor)
        {
            grabbedReactor.Grab();
        }
        target.transform.SetParent(transform);
        grabOffsetPosition = target.transform.position - transform.position;
        grabOffsetRotation = target.transform.rotation * Quaternion.Inverse(transform.rotation);
        //onUpdate = UpdateGrab;

    }

    public void Release(GameObject target)
    {
        onUpdate = delegate { };
        target.transform.SetParent(null);
        grabbedReactor.Release();
        grabbedReactor = null;

    }

    private void UpdateGrab()
    {
        grabbedReactor.GetComponent<Rigidbody>().MovePosition( transform.position + grabOffsetPosition);
        grabbedReactor.GetComponent<Rigidbody>().MoveRotation( transform.rotation * grabOffsetRotation);
    }
}
