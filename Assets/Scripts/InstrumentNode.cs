using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class InstrumentNode : BaseNode
{
    public enum Note { C3, Cs3, D3, Ds3, E3, F3, Fs3, G3, Gs3, A3, As3, B3, C4, Cs4, D4, Ds4, E4, F4, Fs4, G4, Gs4, A4, As4, B4}
    private Note lastNote;
    public Note note;
    public int pitch;
    public InstrumentVFXController vfxController;

    internal override void OnUpdate()
    {
        base.OnUpdate();
        if(lastNote != note)
        {
            ChangePitch(note);
            lastNote = note;
        }
    }

    internal override void OnStart()
    {
        base.OnStart();
        ChangePitch(note);
        lastNote = note;
    }

    public override void NextOption()
    {
        base.NextOption();
        int nextEnum = (int)note + 1;
        if (nextEnum > Enum.GetNames(typeof(Note)).Length - 1) nextEnum = 0;
        note = (Note)nextEnum;
    }

    public override void PrevOption()
    {
        base.PrevOption();
        int nextEnum = (int)note - 1;
        if (nextEnum < 0) nextEnum = Enum.GetNames(typeof(Note)).Length - 1;
        note = (Note)nextEnum;
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Activate( float _duration)
    {
        base.Activate();
        if (helm)
        {

            helm.NoteOn(pitch, 1.0f, _duration*0.95f);
        }
        if (sampler)
        {
            sampler.NoteOn(pitch);

        }
        vfxController.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (helm)
        {
           // helm.NoteOff(pitch);
        }
        if (sampler)
        {
            //sampler.NoteOff();
        }
        vfxController.Deactivate();

    }

    public override BaseNode Duplicate()
    {
        InstrumentNode _node = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<InstrumentNode>();
        _node.childNodes.Clear();
        _node.Deactivate();
        _node.parentTarget = this;
        return _node;
    }

    private void ChangePitch(Note _note)
    {
        pitch = (int)_note + 48;
        vfxController.ChangeNote(_note);
    }
}
