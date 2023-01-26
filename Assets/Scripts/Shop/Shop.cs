using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    public Item[] itemsForSale;

    [Header("References: ")]
    public GameObject shopMenu;
    // public GameObject buyMenu;
    // public GameObject sellMenu;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI gold;
    List<ItemButton> itemsList = new List<ItemButton>();
    public GameObject itemsGrid;
    public ItemButton itemButton;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI useButton;

    private Item selectedItem;
    private Dictionary<Item, int> holdedItems = new Dictionary<Item, int>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            Debug.Log("K was pressed");
            SwitchShopMenu();
        }
        if(shopMenu.activeInHierarchy)
        {
            gold.text = GameManager.instance.CurrentGold.ToString();
        }
    }



    public void SwitchShopMenu()
    {
        // Debug.Log("Menu State Was Switched");
        shopMenu.SetActive(!shopMenu.activeInHierarchy);
        GameManager.instance.gameEvents["Shop Menu"] = shopMenu.activeInHierarchy;

        if(shopMenu.activeInHierarchy)
        {
            SwitchBuyMenu();
        }
    }

    public void SwitchBuyMenu()
    {
        selectedItem = null;
        if(shopMenu.activeInHierarchy)
        {
            useButton.GetComponentInParent<Button>().onClick.RemoveAllListeners();
            useButton.GetComponentInParent<Button>().onClick.AddListener(BuyItem);
            useButton.text = "Buy Item";
            // buyMenu.SetActive(true);
            // sellMenu.SetActive(false);

            if(itemsForSale != null)
            {
                holdedItems.Clear();
                foreach(Item item in itemsForSale)
                {
                    //Debug.Log(item.itemName);
                    if(item != null)
                    {
                        if(holdedItems.ContainsKey(item))
                        {
                            holdedItems[item]++;
                        }else
                        {
                            holdedItems.Add(item, 1);
                        }
                    }
                }
            }
            RefreshShopInventory(holdedItems, "Buy");

        }
    }
    
    public void SwitchSellMenu()
    {
        selectedItem = null;
        if(shopMenu.activeInHierarchy)
        {
            useButton.GetComponentInParent<Button>().onClick.RemoveAllListeners();
            useButton.GetComponentInParent<Button>().onClick.AddListener(SellItem);
            useButton.text = "Sell Item";

            // sellMenu.SetActive(true);
            // buyMenu.SetActive(false);

            RefreshShopInventory(GameManager.instance.playerItems, "Sell");
        }
    }


    public void RefreshShopInventory(Dictionary<Item, int> items, string type)
    {
        if(itemsGrid.activeInHierarchy)
        {
            foreach (Transform child in itemsGrid.transform) 
            {
                // OnInventoryChanged -= child.gameObject.GetComponent<ItemButton>().UpdateInfo;
                GameObject.Destroy(child.gameObject);
            }
            if(GameManager.instance != null)
            {
                itemsList.Clear();
                int counter = 0;
                foreach(KeyValuePair<Item, int> item in items)
                {
                    //Debug.Log(item.Key.itemName);
                    itemsList.Add(Instantiate(itemButton, new Vector3 (0,0,0), Quaternion.identity, itemsGrid.transform));
                    itemsList[counter].UpdateInfo(item.Key, item.Value.ToString());
                    if(type == "Sell")
                    {
                        itemsList[counter].GetComponent<Button>().onClick.AddListener(()=>{UpdateItem(item.Key, item.Key.sellPrice);});
                    }else
                    {
                        itemsList[counter].GetComponent<Button>().onClick.AddListener(()=>{UpdateItem(item.Key, item.Key.buyPrice);});
                    }
                    counter++;
                }
                if(selectedItem == null)
                {
                    itemsList[0].GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }

    public void BuyItem()
    {
        if(selectedItem != null)
        {
            if(GameManager.instance.CurrentGold >= selectedItem.buyPrice)
            {
                GameManager.instance.CurrentGold -= selectedItem.buyPrice;
                GameManager.instance.AddItem(selectedItem);
            }
        }
    }

    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.CurrentGold += selectedItem.sellPrice;
            GameManager.instance.RemoveItem(selectedItem);
            if(!GameManager.instance.playerItems.ContainsKey(selectedItem))
            {
                selectedItem = null;
            }
            RefreshShopInventory(GameManager.instance.playerItems, "Sell");
        }
    }

    private void UpdateItem(Item _item, int price)
    {
        selectedItem = _item;
        itemName.text = _item.itemName;
        itemDesc.text = _item.description;
        itemPrice.text = price.ToString();
    }
}
