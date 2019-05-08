using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    public BaseNode parentNode;
    public string eventName;
    public Color idleColor;
    public Color selectedColor;
    public Color activeColor;
    private Material mat;
    private Vector3 defaultScale;

    void Start()
    {
        parentNode = GetComponentInParent<BaseNode>();
        if(GetComponent<Renderer>()) mat = GetComponent<Renderer>().material;
        defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;
        if(mat)
        {
            mat.SetColor("_Color", idleColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Select()
    {
        if(mat) mat.DOColor(selectedColor, 0.3f);
        //Debug.Log(GetComponent<Renderer>().material.GetColor("Tint"));
    }

    public void Deselect()
    {
        if(mat) mat.DOColor(idleColor, 0.3f);

    }

    public void Show()
    {
        DOTween.Sequence().Append(transform.DOScale(defaultScale, 0.2f));
    }

    public void Hide()
    {
        DOTween.Sequence().Append(transform.DOScale(0f, 0.2f));
    }

    public void Activate()
    {
        parentNode.SendEvent(eventName);
        parentNode.SendEvent("PlaySample");
        if (mat)
        {
            DOTween.Sequence().Append(mat.DOColor(activeColor, 0.1f)).Append(mat.DOColor(selectedColor, 0.3f));
        }

        //GetComponent<Renderer>().material.SetColor("Tint", )
    }
}
