using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TriggerSensor : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> triggeringObjects = new List<GameObject>();
    public Action<GameObject> onTriggerEnter = delegate (GameObject go) { };
    public Action<GameObject> onTriggerExit = delegate (GameObject go) { };

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        triggeringObjects.Add(other.gameObject);
        onTriggerEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        triggeringObjects.Remove(other.gameObject);
        onTriggerExit(other.gameObject);
    }

    public GameObject GetClosest(Vector3 _position, Type _type)
    {
        return triggeringObjects.Where( t => t.GetComponentInParent(_type) != null).OrderBy(t => (t.transform.position - _position).sqrMagnitude).FirstOrDefault();
                           
    }

    public GameObject GetMostRecentEntry()
    {
        if(triggeringObjects.Count > 0)
        {
            return triggeringObjects[triggeringObjects.Count - 1];
        }
        return null; 
    }
}
