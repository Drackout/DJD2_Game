using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUsed : MonoBehaviour
{
    public Interactive Item;
    [SerializeField] private TextMeshProUGUI TotalDone;
    [SerializeField] private Animator anim;

    void Start()
    {
        //Item.onPicked.AddListener(OnItemPicked);
        //Item.onInteracted.AddListener(OnItemInteracted);
    }

    public void OnItemPicked()
    {
        print("The Item was picked.");
    }

    public void OnItemInteracted()
    {
        print("The Item was used.");
        incrementInt();
    }

    public void onRequirementsMet()
    {
        print("Requires met!.");
    }

    void Update()
    {
        checkComplete();
    }

    public void checkComplete()
    {

        //And verifies if they're all inserted or not
        int valConvert;

        valConvert = int.Parse(TotalDone.text);

        if (valConvert >= 4)
        {
            //print("finished!");
            anim.SetTrigger("Interaction");
        }
    }
    public void incrementInt()
    {
        //This +1 when a frame is inserted
        int valConvert;

        valConvert = int.Parse(TotalDone.text);
        valConvert++;

        TotalDone.text = valConvert.ToString();

        print(TotalDone.text);
    }
}
