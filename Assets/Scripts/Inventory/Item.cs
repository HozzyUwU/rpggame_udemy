using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Type Of Item")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;

    [Header("Item Info")]
    public string itemName;
    public string description;
    public int value;
    public int buyPrice;
    public int sellPrice;
    public Sprite itemSprite;

    [Header("Affecting Type")]
    public int affectingValue;
    public bool affectHP;
    public bool affectMP;
    public bool affectSTR;

    [Header("Additional Info")]
    public int weaponStrength;
    public int armorStrength;
    [Header("References")]
    public CharacterInventory inventory;

    public void ApplyItem(CharStats character)
    {
        Debug.Log(itemName + " Was Applayed To " + character.charName);
        if(isItem)
        {
            if(affectHP)
            {
                character.HP += affectingValue;
            }
            else if(affectMP)
            {
                character.MP += affectingValue;
            }
            else if(affectSTR)
            {
                character.strength += affectingValue;
            }
        }

        if(isWeapon)
        {
            if(character.equippedWeapon != null)
            {
                GameManager.instance.AddItem(character.equippedWeapon);
            }
            character.equippedWeapon = this;
            character.wpnPower = weaponStrength;
        }
        else if(isArmor)
        {
            if(character.equippedArmor != null)
            {
                GameManager.instance.AddItem(character.equippedArmor);
            }
            character.equippedArmor = this;
        }

        GameManager.instance.RemoveItem(this);
        //inventory.RefreshInventory();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
