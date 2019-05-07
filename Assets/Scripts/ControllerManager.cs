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
    public TriggerSensor grabSensor;
    public Action onUpdate = () => {};
    private GameObject objectInHand; // 2
    private BaseNode hoveringNode;
    private BaseNode grabbedNode;
    private Vector3 grabOffsetPosition;
    private Quaternion grabOffsetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForHoveringObject();
        if (grabAction.GetLastStateDown(handType))
        {
            if(hoveringNode)
            {
                Grab(hoveringNode);
            }
        }

        if (copyAction.GetLastStateDown(handType))
        {
            if(hoveringNode)
            {
                BaseNode _node = hoveringNode.Duplicate();
                Grab(_node);
            }
        }

        if (copyAction.GetLastStateUp(handType))
        {

                Release(grabbedNode.gameObject);

        }

        // 2
        if (grabAction.GetLastStateUp(handType))
        {
            Release(grabbedNode.gameObject);
        }
        onUpdate();
    }

    private void CheckForHoveringObject()
    {
        if( grabSensor.GetCount(typeof(BaseNode)) > 0 )
        {
            BaseNode _node = grabSensor.GetClosest(transform.position, typeof(BaseNode)).GetComponentInParent<BaseNode>();
            if (hoveringNode)
            {
                // if hovering closer to a different node
                if (_node != hoveringNode)
                {
                    hoveringNode.Deselect();
                }
                else
                {
                    return;
                }
            }
            _node.Select();
            hoveringNode = _node;
        }
        else
        {
            if(hoveringNode)
            {
                hoveringNode.Deselect();
                hoveringNode = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    public void Grab(BaseNode target)
    {
        grabbedNode = target;
        if(grabbedNode)
        {
           grabbedNode.Grab();
        }
        target.transform.SetParent(transform);
        //grabOffsetPosition = target.transform.position - transform.position;
        //grabOffsetRotation = target.transform.rotation * Quaternion.Inverse(transform.rotation);
        //onUpdate = UpdateGrab;

    }

    public void Release(GameObject target)
    {
        onUpdate = delegate { };
        target.transform.SetParent(null);
       // grabbedReactor.Release();
        grabbedNode = null;

    }

    private void UpdateGrab()
    {
        grabbedNode.GetComponent<Rigidbody>().MovePosition( transform.position + grabOffsetPosition);
        grabbedNode.GetComponent<Rigidbody>().MoveRotation( transform.rotation * grabOffsetRotation);
    }
}
