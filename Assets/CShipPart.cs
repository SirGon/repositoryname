using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShipPart : MonoBehaviour
{
    //Saves the original position of the object, so I can re-set the position when respawning.
    private Material _originalMaterial;
    public Vector3 _originalPosition;
    public Quaternion _originalRotation;
    private int _state;
    private const int NORMAL = 0;
    private const int DISSOLVING = 1;
    public bool _dissolvesBool = true;

    private float _counter = 0f;

    private Renderer _rend;

    // Use this for initialization
    void Start ()
    {
        _rend = GetComponent<Renderer>();
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
        if (_dissolvesBool)
        {
            _originalMaterial = _rend.material;
        }

	}

    void Update ()
    {
        if (_state == DISSOLVING)
        {
            if (_counter < 1f)
            {
                _counter += 1f * Time.deltaTime;
                _rend.material.SetFloat("_Level", _counter);
            }

            else
            {
                _counter = 0f;
                _state = NORMAL;
            }
        }
    }

    public void Dissolve (Material _dissolveMaterial)
    {
       _rend.material = _dissolveMaterial;
       _state = DISSOLVING;
    }

    public void Reset()
    {
        if (_dissolvesBool)
        {
            _rend.material = _originalMaterial;
        }
    }
}
