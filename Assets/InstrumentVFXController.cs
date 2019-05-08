using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using DG.Tweening;

public class InstrumentVFXController : MonoBehaviour
{
    public float activationDuration = 0.2f;
    public float activeScale = 0.3f;
    public float idleScale = 0.2f;
    public float scale = 0.2f;

    public VisualEffect vfx;
    public Texture2D noteA;
    public Texture2D noteAs;
    public Texture2D noteB;
    public Texture2D noteC;
    public Texture2D noteCs;
    public Texture2D noteD;
    public Texture2D noteDs;
    public Texture2D noteE;
    public Texture2D noteF;
    public Texture2D noteFs;
    public Texture2D noteG;
    public Texture2D noteGs;

    public Texture2D octave1;
    public Texture2D octave2;
    public Texture2D octave3;
    public Texture2D octave4;
    public Texture2D octave5;
    public Texture2D octave6;
    public Texture2D octave7;
    public Texture2D octave8;


    private InstrumentNode.Note note;
    private int octave;
    private Texture2D currentTexture;
    private Texture2D octaveTexture;
    // Start is called before the first frame update
    void Start()
    {
        UpdateNote();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeNote(InstrumentNode.Note _note, int _octave)
    {
        note = _note;
        octave = _octave;
        UpdateNote();
    }

    public void Activate()
    {
        DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), activeScale, activationDuration);
    }

    public void Deactivate()
    {
        DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), idleScale, activationDuration*1.8f);
    }

    public void Pulsate( float _toScale )
    {
        DOTween.Sequence().Append(DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), idleScale * _toScale, activationDuration)).Append(DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), idleScale, activationDuration));
    }

    private void UpdateNote()
    { 
        switch(note)
        {
            case InstrumentNode.Note.A:
                currentTexture = noteA;
                break;
            case InstrumentNode.Note.As:
                currentTexture = noteAs;
                break;
            case InstrumentNode.Note.B:
                currentTexture = noteB;
                break;
            case InstrumentNode.Note.C:
                currentTexture = noteC;
                break;
            case InstrumentNode.Note.Cs:
                currentTexture = noteCs;
                break;
            case InstrumentNode.Note.D:
                currentTexture = noteD;
                break;
            case InstrumentNode.Note.Ds:
                currentTexture = noteDs;
                break;
            case InstrumentNode.Note.E:
                currentTexture = noteE;
                break;
            case InstrumentNode.Note.F:
                currentTexture = noteF;
                break;
            case InstrumentNode.Note.Fs:
                currentTexture = noteFs;
                break;
            case InstrumentNode.Note.G:
                currentTexture = noteG;
                break;
            case InstrumentNode.Note.Gs:
                currentTexture = noteGs;
                break;
            default:
                currentTexture = noteA;
                break;
        }
        switch (octave)
        {
            case 1:
                octaveTexture = octave1;
                break;
            case 2:
                octaveTexture = octave2;
                break;
            case 3:
                octaveTexture = octave3;
                break;
            case 4:
                octaveTexture = octave4;
                break;
            case 5:
                octaveTexture = octave5;
                break;
            case 6:
                octaveTexture = octave6;
                break;
            case 7:
                octaveTexture = octave7;
                break;
            case 8:
                octaveTexture = octave8;
                break;
            default:
                octaveTexture = octave1;
                break;
        }
        vfx.SetTexture("Texture", currentTexture);
        vfx.SetTexture("OctaveTexture", octaveTexture);
    }
}
