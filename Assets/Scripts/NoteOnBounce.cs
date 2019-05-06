﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteOnBounce : MonoBehaviour
{
    public AudioHelm.HelmController helmController;
    public int note = 60;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        helmController.NoteOn(note);
    }

    private void OnTriggerExit(Collider other)
    {
        helmController.NoteOff(note);
    }
}
