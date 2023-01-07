using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{    
    [SerializeField] private InteractiveData    _interactiveData;
    [SerializeField] private UIManager          _uIManager; //im not proud of this.. but here we go
    [SerializeField] private GameObject         _player;
    [SerializeField] private GameObject         _playerFlashlight;

    private InteractionManager      _interactionManager;
    private PlayerInventory         _PlayerInventory;
    private List<Interactive>       _dependents;
    private List<Interactive>       _requirements;
    private Animator                _animator;
    private bool                    _requirementsMet;
    private int                     _interactionCount;

    public bool                     isOn;
    public UnityEvent onPicked;
    public UnityEvent onRequirementsMet;
    public UnityEvent onInteracted;

    public InteractiveData interactiveData
    {
        get { return _interactiveData; }
    }

    public string inventoryName
    {
        get { return _interactiveData.inventoryName; }
    }

    public int puzzleCode
    {
        get { return  _interactiveData.code; }
    }

    public Sprite inventoryIcon
    {
        get { return _interactiveData.inventoryIcon; }
    }

    public bool requirementsMet
    {
        get { return _requirementsMet; }
    }

    public bool isType(InteractiveData.Type type)
    {
        return _interactiveData.type == type;
    }

    void Awake()
    {
        _interactionManager     = InteractionManager.instance;
        _PlayerInventory        = _interactionManager.playerInventory;
        _requirements           = new List<Interactive>();
        _dependents             = new List<Interactive>();
        _animator               = GetComponent<Animator>();
        _requirementsMet        = interactiveData.requirments.Length == 0;
        _interactionCount       = 0;

        isOn                    = _interactiveData.startsOn;

        _interactionManager.RegisterInteractive(this);
        HideCursor();
        ShowCursor();
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void addRequirement(Interactive requirements)
    {
        _requirements.Add(requirements);
    }

    public void addDependent(Interactive dependent)
    {
        _dependents.Add(dependent);
    }

    public string getInteractionName()
    {
        return interactiveData.inventoryName;
    }

    public string GetInteractionMessage()
    {
        if (isType(InteractiveData.Type.Pickables) && !_PlayerInventory.Contains(this) && requirementsMet)
            return InteractionManager.instance.pickMessage.Replace("%", _interactiveData.inventoryName);
        else if (!requirementsMet)
        {
            if (PlayerHasRequirementSelected())
                return _PlayerInventory.GetInteractionMessage();
            else
                return interactiveData.requirmentsMessage;
        }
        else if (interactiveData.interactionMessages.Length > 0)
            return interactiveData.interactionMessages[_interactionCount % interactiveData.interactionMessages.Length];
        else
            return null;
    }

    private bool PlayerHasRequirementSelected()
    {
        foreach (Interactive requirement in _requirements)
            if (_PlayerInventory.IsSelected(requirement))
                return true;

        return false;
    }

    public void disableMovement()
    {
        _player.GetComponent<PlayerMovement>().enabled = false;
        _playerFlashlight.GetComponent<PlayerFlashlights>().enabled = false;
    }

    public void enableMovement()
    {
        _player.GetComponent<PlayerMovement>().enabled = true;
        _playerFlashlight.GetComponent<PlayerFlashlights>().enabled = true;
    }

    public void Interact()
    {
        if (isType(InteractiveData.Type.Code))
        {
            //Show insert code UI
            _uIManager.ShowInteractionPanel("");
            ShowCursor();
            disableMovement();
            
            //Compare the inserted code with the code itself
            //ValidateCode.cs

        }
        if (requirementsMet)
            InteractSelf(true);
        else if (PlayerHasRequirementSelected())
            UseRequirement();

        
    }

    private void InteractSelf(bool direct)
    {
        //checks if the inventory is full before add
        if (!_PlayerInventory.isFull())
        {
            if (direct && isType(InteractiveData.Type.Indirect))
                return;

            if (isType(InteractiveData.Type.Pickables))
            {
                _PlayerInventory.Add(this);
                gameObject.SetActive(false);
            }
            else if (isType(InteractiveData.Type.Interactive) || isType(InteractiveData.Type.InteractMulti))
            {
                ++_interactionCount;
                UpdateDependents();
                InteractDependents();
            }

            if (isType(InteractiveData.Type.Pickables))
                onPicked.Invoke();
            else
                onInteracted.Invoke();

            if (_animator != null && !isType(InteractiveData.Type.Pickables))
                _animator.SetTrigger("Interact");
        }
    }

    private void UpdateDependents()
    {
        foreach (Interactive dependent in _dependents)
            dependent.CheckRequirements();
    }

    private void CheckRequirements()
    {
        foreach (Interactive requirement in _requirements)
        {
            if (!requirement.requirementsMet ||
               (!requirement.isType(InteractiveData.Type.Indirect)
                && requirement._interactionCount == 0))
            {
                _requirementsMet = false;
                return;
            }
        }
        _requirementsMet = true;

        if (!_requirementsMet)
        {
            _requirementsMet = true;
            onRequirementsMet.Invoke();
        }

        if (_animator != null)
            _animator.SetTrigger("RequirementsMet");
        UpdateDependents();
    }

    private void InteractDependents()
    {
        foreach (Interactive dependent in _dependents)
            if (dependent.requirementsMet && dependent.isType(InteractiveData.Type.Indirect))
                dependent.InteractSelf(false);
    }

    private void UseRequirement()
    {
        Interactive requirement = _PlayerInventory.GetSelected();

        ++requirement._interactionCount;

        requirement.onInteracted.Invoke();

        if (requirement._animator != null)
        {
            requirement.gameObject.SetActive(true);
            requirement._animator.SetTrigger("Interact");
        }

        CheckRequirements();
        _PlayerInventory.Remove(requirement);
        CheckRequirements();
    }
}
