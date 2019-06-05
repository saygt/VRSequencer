using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using AudioHelm;
using System.Collections.Generic;

public class InstrumentNode : BaseNode
{
    public bool useSampler;
    public enum Note { C, Cs, D, Ds, E, F, Fs, G, Gs, A, As, B}
    [Range(1,8)]
    public int octave;
    private int lastOctave;
    private Note lastNote;
    public Note note;
    public int pitch;
    public InstrumentVFXController vfxController;
    public List<Sampler> samplers;
    public List<HelmController> helmInstances;
    public List<Color> instrumentColors;
    private Color currentColor;
    private int helmChannel;

    internal override void OnUpdate()
    {
        base.OnUpdate();
        if(lastNote != note || lastOctave != octave)
        {
            ChangePitch(note, octave);
            lastNote = note;
            lastOctave = octave;
        }
    }

    internal override void OnStart()
    {
        base.OnStart();
        ChangePitch(note, octave);
        lastNote = note;
        lastOctave = octave;
        buttons = GetComponentsInChildren<Button>();
    }

    public override void RightAction()
    {
        base.RightAction();
        int nextEnum = (int)note + 1;
        if (nextEnum > Enum.GetNames(typeof(Note)).Length - 1)
        {
            // if there is a higher octave, move up octave
            if(octave < 8)
            {
                octave++;
                nextEnum = 0;
            }
            else
            {
                return;
            }
        }
        note = (Note)nextEnum;
        ChangePitch(note, octave);
        vfxController.Pulsate(1.1f);
    }

    public override void LeftAction()
    {
        base.LeftAction();
        int nextEnum = (int)note - 1;
        if (nextEnum < 0)
        {
            // if there is a lower octave , move down octave
            if(octave > 1)
            {
                octave--;
                nextEnum = Enum.GetNames(typeof(Note)).Length - 1;
            }
            else
            {
                return;
            }
        }
        note = (Note)nextEnum;
        ChangePitch(note, octave);
        vfxController.Pulsate(0.9f);
    }

    public override void UpAction()
    {
        base.UpAction();
        if (octave < 8)
        {
            octave++;
        }
        ChangePitch(note, octave);
        vfxController.Pulsate(1.1f);

    }

    public override void DownAction()
    {
        base.DownAction();
        if (octave > 1)
        {
            octave--;
        }
        ChangePitch(note, octave);
        vfxController.Pulsate(0.9f);
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Activate( float _duration)
    {
        base.Activate();
        if (!useSampler && helm)
        {
            helm.NoteOn(pitch, 1.0f, _duration * 0.99f);
        }
        if (useSampler && sampler)
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

    public override void Select()
    {
        base.Select();
        foreach (Button button in buttons)
        {
            button.Show();
        }
        PlaySample();
    }

    public override void Deselect()
    {
        foreach (Button button in buttons)
        {
            button.Hide();
        }
        base.Deselect();
    }

    private void PlaySample()
    {
        if ( !useSampler && helm)
        {
            helm.NoteOn(pitch, 1.0f, 0.5f);
        }
        if ( useSampler &&sampler)
        {
            sampler.NoteOn(pitch);
        }
        vfxController.Pulsate(1.1f);
    }

    public override BaseNode Duplicate()
    {
        InstrumentNode _node = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<InstrumentNode>();
        _node.rayInstance = null;
        _node.childNodes.Clear();
        _node.Deactivate();
        _node.parentTarget = this;
        return _node;
    }

    private void ChangePitch(Note _note, int octave)
    {
        pitch = 12*octave + (int)_note;
        vfxController.ChangeNote(_note, octave);
    }

    public override void SendEvent(string _eventName)
    {
        base.SendEvent(_eventName);
        switch(_eventName)
        {
            case "NextNote":
                RightAction();
                break;
            case "PrevNote":
                LeftAction();
                break;
            case "UpOctave":
                UpAction();
                break;
            case "DownOctave":
                DownAction();
                break;
            case "NextInstrument":
                NextInstrument();
                break;
            case "PrevInstrument":
                PrevInstrument();
                break;
            case "PlaySample":
                PlaySample();
                break;
            case "Delete":
                Die();
                break;
        }
    }

    private void PrevInstrument()
    {
        if(useSampler)
        {
            PrevSampler();
            return;
        }
        helmChannel--;
        if (helmChannel < 0)
        {
            helmChannel = helmInstances.Count - 1;
        }
        helm = helmInstances[helmChannel];
        currentColor = instrumentColors[helmChannel];
        vfx.SetVector4("BaseColor", currentColor);
        PlaySample();
    }

    private void PrevSampler()
    {
        Debug.Log("prev sampler");
        helmChannel--;
        if (helmChannel < 0)
        {
            helmChannel = samplers.Count - 1;
        }
        sampler = samplers[helmChannel];
        currentColor = instrumentColors[helmChannel];
        vfx.SetVector4("BaseColor", currentColor);
        PlaySample();
    }

    private void NextInstrument()
    {
        if (useSampler)
        {
            NextSampler();
            return;
        }

        helmChannel++;
        if(helmChannel > helmInstances.Count-1)
        {
            helmChannel = 0;
        }
        helm = helmInstances[helmChannel];
        currentColor = instrumentColors[helmChannel];
        vfx.SetVector4("BaseColor", currentColor);
        PlaySample();
    }

    private void NextSampler()
    {
        helmChannel++;
        if (helmChannel > samplers.Count - 1)
        {
            helmChannel = 0;
        }
        sampler = samplers[helmChannel];
        currentColor = instrumentColors[helmChannel];
        vfx.SetVector4("BaseColor", currentColor);
        PlaySample();
    }

    private void Die()
    {
        if(parentTarget)
        {
            parentTarget.RemoveChild(this);
        }
        if(rayInstance)
        {
            Destroy(rayInstance.gameObject);
        }
        DOTween.Sequence().Append(transform.DOScale(0, 0.5f)).OnComplete(() => Destroy(gameObject));
    }
}
