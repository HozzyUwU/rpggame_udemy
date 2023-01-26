using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [HideInInspector]
    // [Header("Area player is exiting from")]
    // [Space]
    public string transitionName;

    void Start()
    {
        if(transitionName == MyPlayerController.instance.areaTransitionName)
        {
            MyPlayerController.instance.transform.position = transform.position;
        }
    }
}
