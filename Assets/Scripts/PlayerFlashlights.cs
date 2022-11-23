using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlights : MonoBehaviour
{
    private CharacterController _characterController;
    private Transform           _head;
    private Light               _Light;


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
            if (_Light.color == Color.magenta)
            {
                _Light.intensity = 2;
                _Light.color = Color.white;
            }
            else
            {
                _Light.intensity = 1;
                _Light.color = Color.magenta;
                //Call UV mode
            }
        }
    }
    
}
