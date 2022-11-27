using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Interactive fuse;

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
    }
}
