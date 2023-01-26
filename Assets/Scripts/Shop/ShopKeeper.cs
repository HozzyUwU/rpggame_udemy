using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    [Header("Shop Stocks: ")]
    public Item[] items;
    public int[] pricesToBuy;
    public int[] pricesToSell;

    private bool canOpen; 

    // Start is called before the first frame update
    void Start()
    {
        int counter = 0;
        foreach(Item item in items)
        {
            //item.shopPrice = prices[counter];
            counter++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpen && Input.GetButtonDown("Fire1") && MyPlayerController.instance.canMove)
        {
            int counter = 0;
            foreach(Item item in items)
            {
                item.buyPrice = pricesToBuy[counter];
                item.sellPrice = pricesToSell[counter];
                counter++; 
            }
            Shop.instance.itemsForSale = items;
            Shop.instance.SwitchShopMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }        
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canOpen = false;
        }    
    }
}
