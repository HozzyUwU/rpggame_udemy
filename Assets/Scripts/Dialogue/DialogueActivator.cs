using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [Header("Character's strings: ")]
    public string[] lines;
    [Header("Quests To Start")]
    public string[] questsToStart;
    [Header("Quests To Complete")]
    public string[] questsToComplete;

    // booleans
    private bool canActivate;
    public bool isPerson = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canActivate && Input.GetKeyUp(KeyCode.E))
        {
            DialogueScript.instance.SetDialogue(lines, isPerson, questsToComplete, questsToStart);
            //canActivate = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canActivate = true;
        }    
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
