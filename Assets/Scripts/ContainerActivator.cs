using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ContainerActivator : MonoBehaviour
{
    public Vector3 startPosition = Vector3.zero;
    public float fieldHeight;
    public float loopDuration;
    public Action onUpdate = delegate { };
    private Rigidbody rb;
    private float velocityScale;
    private float currentTimeInLoop = 0;
    // Start is called before the first frame update
    void Start()
    {
        onUpdate = UpdatePosition;
        rb = GetComponent<Rigidbody>();
        velocityScale = fieldHeight / loopDuration;
    }

    // Update is called once per frame
    void Update()
    {
        onUpdate();
    }

    private void UpdatePosition()
    {
        currentTimeInLoop += Time.deltaTime;
        if(currentTimeInLoop > loopDuration)
        {
            currentTimeInLoop = currentTimeInLoop - loopDuration;
            rb.position = startPosition + Vector3.up * currentTimeInLoop;
        }
        else
        {
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y + ( Time.deltaTime * velocityScale), rb.position.z));
        }
    }
}
