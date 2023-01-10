using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlights : MonoBehaviour
{
    [SerializeField] private float _lightIntensityNormal;
    [SerializeField] private float _lightIntensityUV;
    [SerializeField] private Color _lightColorNormal;
    [SerializeField] private Color _lightColorUV;


    [SerializeField] private Camera _camera;

    [SerializeField] private LayerMask currentLayer;


    private Light               _light;
    private bool                _isUvOn;


    private void ShowUV()
    {
        _camera.cullingMask |= 1 << LayerMask.NameToLayer("UV");
    }

    private void HideUV()
    {
        _camera.cullingMask &= ~(1 << LayerMask.NameToLayer("UV"));
    }

    void Start()
    {
        _light = GetComponent<Light>();
        _isUvOn = false;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _light.enabled = !_light.enabled;
            if (_isUvOn && _light.enabled)
                ShowUV();
            else
                HideUV();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (_light.color == _lightColorUV)
            {
                _isUvOn = false;
                HideUV();
                _light.intensity = _lightIntensityNormal;
                _light.color = _lightColorNormal;
            }
            else
            {
                _isUvOn = true;
                if (_light.enabled == true)
                    ShowUV();

                _light.intensity = _lightIntensityUV;
                _light.color = _lightColorUV;
            }
        }
    }
    
}
