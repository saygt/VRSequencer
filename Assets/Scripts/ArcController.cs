using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;
using UnityEngine;

[ExecuteInEditMode]
public class ArcController : MonoBehaviour
{
    public VisualEffect vfx;
    public Transform owner;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localizedTargetPosition = target.position - owner.position;
        Vector3 localizedOwnerPosition = owner.position;
        vfx.SetVector3("TargetPosition", target.position);
        vfx.SetVector3("OwnerPosition", owner.position);
//        transform.LookAt(target);
    }

    public void SetOwner ( Transform _owner )
    {
        owner = _owner;
    }

    public void SetTarget ( Transform _target )
    {
        target = _target;
    }

}
