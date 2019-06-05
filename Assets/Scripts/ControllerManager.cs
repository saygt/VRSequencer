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
    public SteamVR_Action_Vector2 scrollAction;
    public SteamVR_Action_Boolean selectRightAction;
    public SteamVR_Action_Boolean selectLeftAction;
    public SteamVR_Action_Boolean selectUpAction;
    public SteamVR_Action_Boolean selectDownAction;
    public PointerController pointer;
    public Action onUpdate = () => {};
    private GameObject objectInHand; // 2
    private BaseNode selectedNode;
    private Button selectedButton;
    private BaseNode grabbedNode;
    private Vector3 grabOffsetPosition;
    private Quaternion grabOffsetRotation;
    private float initialTouchAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onUpdate();

        GetSelectedButton();
        if(selectedButton)
        {
            if (grabAction.GetLastStateDown(handType))
            {
                Button _button = pointer.GetSelectedButton();
                if (_button)
                {
                    _button.Activate();
                }
            }
            Debug.Log("1");
            return;
        }
        GetSelectedNode();
        if (selectedNode )
        {
            if (grabAction.GetLastStateDown(handType))
            {
                Button _button = pointer.GetSelectedButton();
                if (_button)
                {
                    _button.Activate();
                }
                Grab(selectedNode);
            }
            if (copyAction.GetLastStateDown(handType))
            {
                BaseNode _node = selectedNode.Duplicate();
                Grab(_node);
            }
            if (selectRightAction.GetLastStateDown(handType))
            {
                selectedNode.RightAction();

            }
            if (selectLeftAction.GetLastStateDown(handType))
            {
                selectedNode.LeftAction();

            }
            if (selectUpAction.GetLastStateDown(handType))
            {
                selectedNode.UpAction();
            }
            if (selectDownAction.GetLastStateDown(handType))
            {
                selectedNode.DownAction();
            }
            if(scrollAction.GetAxis(handType) != Vector2.zero)
            {
                //Debug.Log(scrollAction.GetAxis(handType));
            }
 
            //if(scrollValue.y > 0)
            //{
            //    hoveringNode.RightAction();
            //}
            //else if(scrollValue.y < 0)
            //{
            //    hoveringNode.LeftAction();
            //}

        }

        if (copyAction.GetLastStateUp(handType))
        {
            if (grabbedNode)
            {
                Release(grabbedNode.gameObject);
            }

        }

        // 2
        if (grabAction.GetLastStateUp(handType))
        {
            if(grabbedNode)
            {
                Release(grabbedNode.gameObject);
            }
            
        }
    }

    private void GetSelectedButton()
    {
        Button _selectedButton = pointer.GetSelectedButton();
        if (_selectedButton)
        {
            if (selectedButton)
            {
                // if hovering closer to a different node
                if (_selectedButton != selectedButton)
                {
                    selectedButton.Deselect();
                }
                else
                {
                    return;
                }
            }
            _selectedButton.Select();
            selectedButton = _selectedButton;
        }
        else
        {
            if (selectedButton)
            {
                selectedButton.Deselect();
                selectedButton = null;
            }
        }
    }

    private void GetSelectedNode()
    {
        BaseNode _selectTarget = pointer.GetSelectedTarget();
        if( _selectTarget )
        {
            if (selectedNode)
            {
                // if hovering closer to a different node
                if (_selectTarget != selectedNode)
                {
                    selectedNode.Deselect();
                }
                else
                {
                    return;
                }
            }
            _selectTarget.Select();
            selectedNode = _selectTarget;
        }
        else
        {
            if(selectedNode)
            {
                selectedNode.Deselect();
                selectedNode = null;
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
        pointer.Lock(true);
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
        pointer.Lock(false);
        onUpdate = delegate { };
        target.transform.SetParent(null);
        grabbedNode.Release();
        grabbedNode = null;

    }

    private void UpdateGrab()
    {
        grabbedNode.GetComponent<Rigidbody>().MovePosition( transform.position + grabOffsetPosition);
        grabbedNode.GetComponent<Rigidbody>().MoveRotation( transform.rotation * grabOffsetRotation);
    }
}

public static class _Vector2
{
    public static float AngleTo(this Vector2 this_, Vector2 to)
    {
        Vector2 direction = to - this_;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }
}
