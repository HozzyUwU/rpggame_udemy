using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CharacterInventory : MonoBehaviour
{
    // Creating delegate to notify other objects that inventory was changed
    public delegate void InventoryChanged(Item _item);
    public static event InventoryChanged OnInventoryChanged;
    List<Button> itemsList = new List<Button>();
    public GameObject itemsGrid;
    public Button itemButton;

    [Header("All Game Items")]
    public Item[] items;

    [Header("References")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI useButton;
    public GameObject dropdownButton;
    public GameObject chooseMenu;
    public TextMeshProUGUI useText;
    ItemButton itemInstance;
    private Item selectedItem;

     private void Awake()
    {
        //Debug.Log("Hello");
        GameManager.OnInventoryRefresh -= RefreshInventory;
        GameManager.OnInventoryRefresh += RefreshInventory;
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.B))
        {
            AddSelectedItem(items[0]);
        }
        if(Input.GetKeyUp(KeyCode.N))
        {
            AddSelectedItem(items[1]);
        }
        if(Input.GetKeyUp(KeyCode.M))
        {
            AddSelectedItem(items[2]);
        }
        if(Input.GetKeyUp(KeyCode.P))
        {
            foreach (Transform child in itemsGrid.transform) 
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void AddSelectedItem(Item _item)
    {
        GameManager.instance.AddItem(_item);
        // RefreshInventory();
    }

    public void RemoveSelectedItem()
    {
        if(selectedItem != null && GameManager.instance.playerItems.ContainsKey(selectedItem))
        {
            GameManager.instance.RemoveItem(selectedItem);
        }
        // RefreshInventory();
    }

    public void UseItem()
    {
        TMP_Dropdown dropdown = dropdownButton.GetComponent<TMP_Dropdown>();
        // Debug.Log(dropdown.value);
        if(selectedItem != null && GameManager.instance.playerItems.ContainsKey(selectedItem))
        {
            if(!BattleManager.instance.isActive)
            {
                CharStats temp = null;
                foreach (CharStats character in GameManager.instance.playerStats)
                {
                    if(character.charName == dropdown.options[dropdown.value].text)
                    {
                        temp = character;
                    }
                }
                selectedItem.ApplyItem(temp);
            }else
            {
                BattleManager.instance.UseItem(selectedItem, dropdown.options[dropdown.value].text);
                GameManager.instance.RemoveItem(selectedItem);
            }
        }
        
        ShowChooseMenu();
    }
    
    public void ShowChooseMenu()
    {
        chooseMenu.SetActive(!chooseMenu.activeSelf);
        if(chooseMenu.activeSelf)
        {
            useText.text = "Choose character to use " + selectedItem.itemName + " for:";
            TMP_Dropdown dropdown = dropdownButton.GetComponent<TMP_Dropdown>();
            List<string> playersOptions = new List<string>();
            if(!BattleManager.instance.isActive)
            {
                foreach (CharStats character in GameManager.instance.playerStats)
                {
                    playersOptions.Add(character.charName);
                }
            }else
            {
                foreach(BattleChar battler in BattleManager.instance.activeBattlers)
                {
                    if(battler.isPlayer)
                    {
                        playersOptions.Add(battler.charName);
                    }
                }
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(playersOptions);
        }
    }

    public void RefreshInventory()
    {
        if(itemsGrid != null)
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
                    foreach(KeyValuePair<Item, int> item in GameManager.instance.playerItems)
                    {
                        itemsList.Add(Instantiate(itemButton, new Vector3 (0,0,0), Quaternion.identity, itemsGrid.transform));
                        // itemInstance = itemsList[counter].GetComponent<ItemButton>();
                        if(OnInventoryChanged != null)
                        {
                            // Debug.Log("Event was called");
                            OnInventoryChanged(item.Key);
                        }
                        // itemsList[counter].GetComponentInChildren<TextMeshProUGUI>().text = item.Value.ToString();
                        // itemsList[counter].transform.Find("Item Image").gameObject.GetComponent<Image>().sprite = item.Key.itemSprite;
                        // itemInstance.playerItem = item.Key;
                        itemsList[counter].GetComponent<Button>().onClick.AddListener(()=>{UpdateItem(item.Key);});
                        counter++;
                    }
                }
            }
        }
    }

    private void UpdateItem(Item _item)
    {
        selectedItem = _item;
        Debug.Log(selectedItem);
        if(_item.isItem)
        {
            useButton.text = "Use";
        }else
        {
            useButton.text = "Equip";
        }
        itemName.text = _item.itemName;
        itemDesc.text = _item.description;
    }

}
