using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item_Used : MonoBehaviour
{
    public Interactive fuse;
    [SerializeField] private TextMeshProUGUI TotalDone;
    [SerializeField] private Animator anim;

    void Start()
    {
        //fuse.onPicked.AddListener(OnFusePicked);
        //fuse.onInteracted.AddListener(OnFuseInteracted);
    }

    public void OnFusePicked()
    {
        print("The fuse was picked.");
    }

    public void OnFuseInteracted()
    {
        print("The fuse was used.");
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
            print("noice");
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
