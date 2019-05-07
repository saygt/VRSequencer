using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalSpawn : MonoBehaviour
{

    public GameObject spawnObject;
    public int waitBeats;
    private int currentBeat;
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Instance.DoOnTick(OnBeat);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeat(int _beat)
    {
        if(_beat % waitBeats == 0)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Instantiate(spawnObject, transform.position, transform.rotation);
    }
}
