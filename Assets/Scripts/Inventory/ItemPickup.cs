using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    bool onPickup = false;

    private void Update() 
    {
        if(onPickup && MyPlayerController.instance.canMove && Input.GetButtonUp("Fire1"))
        {
            foreach(Item item in GameManager.instance.availableItems)
            {
                if(item.itemName == gameObject.GetComponent<Item>().itemName)
                {
                    GameManager.instance.AddItem(item);
                    Destroy(gameObject);
                    return;
                }
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            onPickup = true;    
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            onPickup = false;    
        }
    }
}
