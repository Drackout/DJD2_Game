using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetButtonText : MonoBehaviour
{
    [SerializeField] private GameObject         _button;
    [SerializeField] private TextMeshProUGUI    _answear;


    public void sayNumber(Button btn)
    {
        print(btn.name);
        if (_answear.text.Length < 4)
        {
            _answear.text += btn.name;
        }
    }
}
