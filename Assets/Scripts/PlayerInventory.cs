using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager  _uiManager;
    [SerializeField] private int        _slotCount;
    [SerializeField] private GameObject _gameObjectTest;
    [SerializeField] private Transform  _ObjectViewSpawn;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerFlashlight;

    private List<Interactive>           _inventory;
    private int                         _selectedSlot;
    private bool                        _checkInspector;
    private GameObject                  _obj;
    private Vector3                     _oldPosition;
    private Quaternion                  _oldRotation;

    void Start()
    {
        _inventory = new List<Interactive>();
        _selectedSlot = -1;
        _checkInspector = false;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void disableMovement()
    {
        _player.GetComponent<PlayerMovement>().enabled = false;
        //_playerFlashlight.GetComponent<PlayerFlashlights>().enabled = false;
    }

    public void enableMovement()
    {
        _player.GetComponent<PlayerMovement>().enabled = true;
        //_playerFlashlight.GetComponent<PlayerFlashlights>().enabled = true;
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
            _obj = _inventory[_selectedSlot].gameObject;
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
            {
                SelectInventorySlot(i);
               // print("CLICK");
            }
            else if (Input.GetKey(KeyCode.Alpha1 + i))
            {
                if (_checkInspector == false)
                {
                    // while holding disable object interaction
                    // places it on player position
                    print("HOLD -> " + _inventory[_selectedSlot].getInteractionName());
                    _oldPosition = _obj.transform.position;
                    _oldRotation = _obj.transform.rotation;

                    _obj.GetComponent<Interactive>().isOn = false;
                    if (_obj.GetComponent<ObjectView>())
                    {
                        _obj.GetComponent<ObjectView>().enabled = true;
                    }
                    _obj.SetActive(true);
                    _obj.transform.position = _ObjectViewSpawn.position;
                    _checkInspector = true;
                    disableMovement();
                }
            }
            else if (Input.GetKeyUp(KeyCode.Alpha1 + i))
            {
                print("UP!");

                _obj.transform.position = _oldPosition;
                _obj.transform.rotation = _oldRotation;

                _obj.SetActive(false);
                if (_obj.GetComponent<ObjectView>())
                {
                    _obj.GetComponent<ObjectView>().enabled = false;
                }
                _checkInspector = false;
                enableMovement();
            }
    }

}
