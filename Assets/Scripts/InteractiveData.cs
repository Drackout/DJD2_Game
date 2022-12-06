using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create scriptable object 
[CreateAssetMenu(fileName = "InteractiveData", 
                 menuName = "InteractiveData")]
public class InteractiveData : ScriptableObject
{
    public enum Type { Pickables, Interactive, InteractMulti, Indirect, Code };

    public Type                 type;
    public bool                 startsOn = true;
    public string               inventoryName;
    public Sprite               inventoryIcon;
    public InteractiveData[]    requirments;
    public string               requirmentsMessage;
    public string[]             interactionMessages;
    public int                  code;
}
