using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ReactorContainer : MonoBehaviour
{
    public List<AudioReactor> reactors = new List<AudioReactor>();
    public Color defaultColor;
    public Color poweredColor;
    public Color playColor;
    public float delay;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInChildren<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(AudioReactor target)
    {
        reactors.Add(target);
    }

    public void Remove(AudioReactor target)
    {
        reactors.Remove(target);
    }

    public void Activate()
    {
        DOVirtual.DelayedCall(delay, ()=> mat.color = playColor);
        Debug.Log("tick ");
        foreach (AudioReactor ar in reactors)
        {
            ar.PlayNote(true);
        }
    }

    public void Deactivate()
    {
        mat.color = defaultColor;
        foreach (AudioReactor ar in reactors)
        {
            ar.PlayNote(false);
        }
    }
}
