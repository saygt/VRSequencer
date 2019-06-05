using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = FindObjectOfType<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerCam.transform, Vector3.up);

    }
}
