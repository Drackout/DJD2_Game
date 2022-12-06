using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _interactionPanel;
    [SerializeField] private GameObject _interactionCodePanel;
    [SerializeField] private TextMeshProUGUI _interactionText;
    [SerializeField] private Image[] _inventorySlots;
    [SerializeField] private Image[] _inventoryIcons;

    void Start()
    {
        HideInteractionPanel();
        HideInventoryIcons();
        SetSelectedInventorySlot(-1);
        HideInteractionCodePanel();
    }

    public void HideInteractionPanel()
    {
        _interactionPanel.SetActive(false);
    }

    public void HideInteractionCodePanel()
    {
        _interactionCodePanel.SetActive(false);
    }

    public void ShowInteractionPanel(string message)
    {
        _interactionText.text = message;
        _interactionPanel.SetActive(true);
    }

    public void ShowInteractionCodePanel()
    {
        _interactionPanel.SetActive(true);
        _interactionCodePanel.SetActive(true);
    }

    public void HideInventoryIcons()
    {
        foreach (Image image in _inventoryIcons)
        {
            image.enabled = false;
        }
    }

    public void ShowInventoryIcon(int Index, Sprite icon)
    {
        _inventoryIcons[Index].sprite = icon;
        _inventoryIcons[Index].enabled = true;
    }

    public void SetSelectedInventorySlot(int index)
    {
        foreach (Image image in _inventorySlots)
        {
            Color color = image.color;
            color.a = 0.3f;
            image.color = color;
        }
        if (index >= 0)
        {
            Color color     = _inventorySlots[index].color;
            color.a         = 1.0f;

            _inventorySlots[index].color = color;
        }
    }
}
