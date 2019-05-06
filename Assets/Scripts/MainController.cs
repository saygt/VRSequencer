using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MainController : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean teleportAction; // 2
    public SteamVR_Action_Boolean grabAction; // 3
    // Start is called before the first frame update
    void Start()
    {
        Valve.VR.OpenVR.System.ResetSeatedZeroPose();
    }

    public bool GetGrab() // 2
    {
        return grabAction.GetState(handType);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
