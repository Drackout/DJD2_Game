using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    
    [SerializeField] private InteractiveData _interactiveData;

    private InteractionManager      _interactionManager;
    private PlayerInventory         _PlayerInventory;
    private List<Interactive>       _dependents;
    private List<Interactive>       _requirements;
    private Animator                _animator;
    private bool                    _requirementsMet;
    private int                     _interactionCount;

    public bool                     isOn;

    public InteractiveData interactiveData
    {
        get { return _interactiveData; }
    }

    public string inventoryName
    {
        get { return _interactiveData.inventoryName; }
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
    }

    public void addRequirement(Interactive requirements)
    {
        _requirements.Add(requirements);
    }

    public void addDependent(Interactive dependent)
    {
        _dependents.Add(dependent);
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

    public void Interact()
    {
        if (requirementsMet)
            InteractSelf(true);
        else if (PlayerHasRequirementSelected())
            UseRequirement();
        
    }

    private void InteractSelf(bool direct)
    {
        if (direct && isType(InteractiveData.Type.Indirect))
            return;

        if (_animator != null && !isType(InteractiveData.Type.Pickables))
            _animator.SetTrigger("Interact");

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

        _PlayerInventory.Remove(requirement);

        ++requirement._interactionCount;

        if (requirement._animator != null)
        {
            requirement.gameObject.SetActive(true);
            requirement._animator.SetTrigger("Interact");
        }

        CheckRequirements();
    }
}
