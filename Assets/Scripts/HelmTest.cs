using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HelmTest : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioHelm.HelmPatch patch1;
    public AudioHelm.HelmPatch patch2;
    public AudioHelm.HelmPatch patch3;
    public AudioHelm.HelmController controller;
    void Start()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => PlayPatch(patch1, 2f))
            //.AppendInterval(0.25f)
            //.AppendCallback(() => PlayPatch(patch2, 1f))
            //.AppendInterval(0.25f)
            //.AppendCallback(() => PlayPatch(patch3))
            //.AppendInterval(0.5f)
            //.AppendCallback(() => PlayPatch(patch1))
            ;
    }

    public void PlayPatch(AudioHelm.HelmPatch patch, float dur)
    {
        controller.LoadPatch(patch);
        controller.NoteOn(45, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
