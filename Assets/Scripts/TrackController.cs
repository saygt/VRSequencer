using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class TrackController : MonoBehaviour
{
    // Start is called before the first frame update
    public int ticksPerContainer = 1;
    public List<AudioReactor> containers = new List<AudioReactor>();
    public float delay = 0.01f;
    private int containerIndex = 0;
    private AudioReactor activeReactor;


    void Start()
    {
        //containers = GetComponentsInChildren<ReactorContainer>().OrderBy(go => go.gameObject.name).ToList();
        MusicManager.Instance.AddUpdateAction(OnTick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTick( int tick)
    {
        if ( tick % ticksPerContainer == 0)
        {
            if (activeReactor) activeReactor.PlayNote(false);
            activeReactor = containers[containerIndex];
            DOVirtual.DelayedCall(delay, () => activeReactor.PlayNote(true));
            containerIndex = containerIndex >= containers.Count-1 ? containerIndex = 0 : containerIndex + 1 ;
        }
    }
}
