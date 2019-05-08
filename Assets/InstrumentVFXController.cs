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
    public Texture2D sharpSymbol;
    private InstrumentNode.Note note;
    private Texture2D currentTexture;
    // Start is called before the first frame update
    void Start()
    {
        UpdateNote();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeNote(InstrumentNode.Note _note)
    {
        note = _note;
        UpdateNote();
    }

    public void Activate()
    {
        DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), activeScale, activationDuration);
    }

    public void Deactivate()
    {
        DOTween.To(() => vfx.GetFloat("scale"), x => vfx.SetFloat("scale", x), idleScale, activationDuration);
    }

    private void UpdateNote()
    { 
        switch(note)
        {
            case InstrumentNode.Note.A3:
            case InstrumentNode.Note.A4:
                currentTexture = noteA;
                break;
            case InstrumentNode.Note.As3:
            case InstrumentNode.Note.As4:
                currentTexture = noteAs;
                break;
            case InstrumentNode.Note.B3:
            case InstrumentNode.Note.B4:
                currentTexture = noteB;
                break;
            case InstrumentNode.Note.C3:
            case InstrumentNode.Note.C4:
                currentTexture = noteC;
                break;
            case InstrumentNode.Note.Cs3:
            case InstrumentNode.Note.Cs4:
                currentTexture = noteCs;
                break;
            case InstrumentNode.Note.D3:
            case InstrumentNode.Note.D4:
                currentTexture = noteD;
                break;
            case InstrumentNode.Note.Ds3:
            case InstrumentNode.Note.Ds4:
                currentTexture = noteDs;
                break;
            case InstrumentNode.Note.E3:
            case InstrumentNode.Note.E4:
                currentTexture = noteE;
                break;
            case InstrumentNode.Note.F3:
            case InstrumentNode.Note.F4:
                currentTexture = noteF;
                break;
            case InstrumentNode.Note.Fs3:
            case InstrumentNode.Note.Fs4:
                currentTexture = noteFs;
                break;
            case InstrumentNode.Note.G3:
            case InstrumentNode.Note.G4:
                currentTexture = noteG;
                break;
            case InstrumentNode.Note.Gs3:
            case InstrumentNode.Note.Gs4:
                currentTexture = noteGs;
                break;
            default:
                currentTexture = noteA;
                break;
        }
        vfx.SetTexture("Texture", currentTexture);
    }
}
