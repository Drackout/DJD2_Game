using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float      _maxInteractionDistance;
    [SerializeField] private UIManager  _uiManager;
    
    private Transform   _cameraTransform;
    private Interactive _currentInteractive;
    private bool        _refreshInteractive;

    void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _currentInteractive = null;
        _refreshInteractive = false;
    }

    void Update()
    {
        UpdateCurrentInteractive();
        CheckForPlayerInput();
    }

    private void UpdateCurrentInteractive()
    {
        //Debug.DrawRay(_cameraTransform.position, _maxInteractionDistance * _cameraTransform.forward);
        if(Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, 
                        out RaycastHit hitInfo, _maxInteractionDistance))
        {
            Interactive interactive = hitInfo.collider.GetComponent<Interactive>();
            if(interactive == null || !interactive.isOn)
                ClearCurrentInteractive();
            else if (interactive != _currentInteractive || _refreshInteractive)
                SetCurrentInteractive(interactive);
        }
        else if(_currentInteractive != null)
            ClearCurrentInteractive();
    }

    private void ClearCurrentInteractive()
    {
        _currentInteractive = null;
        _uiManager.HideInteractionPanel();
        _uiManager.HideInteractionCodePanel();
    }

    private void SetCurrentInteractive(Interactive interactive)
    {
        _currentInteractive = interactive;
        _refreshInteractive = false;

        string interactionMessage = interactive.GetInteractionMessage();

        if (interactionMessage != null)
        {
            _uiManager.ShowInteractionPanel(interactionMessage);
        }
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetButtonDown("Pick") && _currentInteractive != null)
        {
            _currentInteractive.Interact();
            _refreshInteractive = true;
        }
    }

    
}
