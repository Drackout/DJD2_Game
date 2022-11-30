using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlights : MonoBehaviour
{
    private CharacterController _characterController;
    private Transform           _head;
    private Light               _Light;

    [SerializeField] private float _lightIntensityNormal;
    [SerializeField] private float _lightIntensityUV;
    [SerializeField] private Color _lightColorNormal;
    [SerializeField] private Color _lightColorUV;


    // Start is called before the first frame update
    void Start()
    {
        _Light = GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _Light.enabled = !_Light.enabled;
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (_Light.color == _lightColorUV)
            {
                _Light.intensity = _lightIntensityNormal;
                _Light.color = _lightColorNormal;
            }
            else
            {
                _Light.intensity = _lightIntensityUV;
                _Light.color = _lightColorUV;
                //Call UV mode
            }
        }
    }
    
}
