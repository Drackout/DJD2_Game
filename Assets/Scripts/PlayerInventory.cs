using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager  _uiManager;
    [SerializeField] private int        _slotCount;

    private List<Interactive>           _inventory;
    private int                         _selectedSlot;

    void Start()
    {
        _inventory = new List<Interactive>();
        _selectedSlot = -1;
    }

    public void Add(Interactive item)
    {
        if (_inventory.Count < 2)
        {
            _inventory.Add(item);
            _uiManager.ShowInventoryIcon(_inventory.Count-1, item.inventoryIcon);

            if (_inventory.Count == 1)
                SelectInventorySlot(0);
        }
        print("inventory: " + _inventory.Count);
    }

    public bool isFull()
    {
        if (_inventory.Count < 2)
            return false;
        else
            return true;
    }

    public void Remove(Interactive item)
    {
        _inventory.Remove(item);
        _uiManager.HideInventoryIcons();
        for (int i = 0; i < _inventory.Count; ++i)
            _uiManager.ShowInventoryIcon(i, _inventory[i].inventoryIcon);

        if (_selectedSlot == _inventory.Count)
            SelectInventorySlot(_selectedSlot-1);
    }

    public bool Contains(Interactive item)
    {
        return _inventory.Contains(item);
    }

    private void SelectInventorySlot(int index)
    {
        //if (index < _inventory.Count)
        {
            _selectedSlot = index;
            _uiManager.SetSelectedInventorySlot(_selectedSlot);
        }
    }

    public string GetInteractionMessage()
    {
        return _inventory[_selectedSlot].GetInteractionMessage();
    }

    public bool IsSelected(Interactive interactive)
    {
        return _selectedSlot != -1 && _inventory[_selectedSlot] == interactive;
    }

    void Update()
    {
        CheckForPlayerInput();    
    }

    public Interactive GetSelected()
    {
        return _selectedSlot != -1 ? _inventory[_selectedSlot] : null;
    }

    private void CheckForPlayerInput()
    {
        for (int i = 0; i < _slotCount; ++i)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectInventorySlot(i);
    }

}
