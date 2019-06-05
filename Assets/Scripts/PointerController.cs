using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class PointerController : MonoBehaviour
{
    private RaycastHit hitInfo;
    public LayerMask sensorLayer;
    public LayerMask buttonLayer;
    public VisualEffect vfx;
    public Color defaultColor;
    public Color interactiveColor;
    private bool locked;
    private BaseNode selectedTarget;
    private Button selectedButton;
    private float length;
    private Color beamColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (locked) return;
        beamColor = defaultColor;
        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray, out hitInfo, 100f, sensorLayer))
        {
            length = hitInfo.distance;
            BaseNode _node = hitInfo.collider.GetComponentInParent<BaseNode>();
            if(_node)
            {
                selectedTarget = _node;
                beamColor = interactiveColor;
            }
        }
        else
        {
            length = 100f;
            selectedTarget = null;
        }

        if(Physics.Raycast(ray, out hitInfo, 100f, buttonLayer))
        {
            length = hitInfo.distance;
            Button _button = hitInfo.collider.GetComponent<Button>();
            if(_button)
            {
                selectedButton = _button;
                beamColor = interactiveColor;
            }
        }
        else
        {
            selectedButton = null;
        }

        vfx.SetFloat("Length", length);
        vfx.SetVector4("Color", beamColor);
    }

    public void Lock( bool _value)
    {
        locked = _value;
    }

    public BaseNode GetSelectedTarget()
    {
        return selectedTarget;
    }

    public Button GetSelectedButton()
    {
        return selectedButton;
    }
}
