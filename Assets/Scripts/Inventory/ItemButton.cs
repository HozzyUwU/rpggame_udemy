using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    [Header("Info To Display")]
    public Image itemImage;
    public TextMeshProUGUI itemCount;

    [Header("References")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    private string _itemName = null, _itemDescription = null;
    private Item item;

    private void Awake()
    {
        //Debug.Log("Hello");
        CharacterInventory.OnInventoryChanged -= UpdateInfo;
        CharacterInventory.OnInventoryChanged += UpdateInfo;
    }

    public void DisplayInfo()
    {
        if(item != null)
        {
            GameMenu.instance.DisplaySelectedItemInfo(item);
        }
    }

    public void UpdateInfo(Item _item)
    {
        CharacterInventory.OnInventoryChanged -= UpdateInfo;
        if(_item != null)
        {
            item = _item;
            GameManager.instance.test++;
            // Debug.Log(_item.itemName);
            itemImage.sprite = _item.itemSprite;
            itemCount.text = GameManager.instance.playerItems[_item].ToString();

            // itemCount.text = _item.itemName;
            _itemName = _item.itemName;
            _itemDescription = _item.description;
        }
    }

    public void UpdateInfo(Item _item, string amount)
    {
        CharacterInventory.OnInventoryChanged -= UpdateInfo;
        if(_item != null)
        {
            item = _item;
            GameManager.instance.test++;
            // Debug.Log(_item.itemName);
            itemImage.sprite = _item.itemSprite;
            itemCount.text = amount;

            // itemCount.text = _item.itemName;
            _itemName = _item.itemName;
            _itemDescription = _item.description;
        }
    }
}
