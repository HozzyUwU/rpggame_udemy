using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [Header("References")]
    [Space]
    public GameObject player;

    void Start()
    {
        if(MyPlayerController.instance == null)
        {
            Instantiate(player);
        }
    }
}
