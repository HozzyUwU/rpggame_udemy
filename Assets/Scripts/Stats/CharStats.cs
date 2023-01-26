using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    // Creating delegate to notify other objects that stats was changed
    public delegate void StatsChanged(CharStats stats);
    public static event StatsChanged OnStatsChanged;

    [Header("Name Of Character")]
    public string charName;
    public Sprite chrImage;

    [Header("Leveling Up")]
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int startExp = 1000;

    //[Header("Health Points")]
    public int HP
    {
        get {return currentHP;}
        set 
        {
            currentHP = value;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        } 
    }
    public int currentHP;
    public int maxHP = 100;

    //[Header("Magick Points")]
    public int MP
    {
        get {return currentMP;}
        set 
        {
            currentMP = value;
            if (currentMP > maxMP)
            {
                currentMP = maxMP;
            }
            if(OnStatsChanged != null)
            {
                OnStatsChanged(this);
            }
        } 
    }
    public int currentMP;
    public int maxMP = 30;
    public int[] mpToAddNextLevel;

    [Header("Stats")]
    public int strength;
    public int defence;
    public int wpnPower;
    public int armrPower;

    [Header("Equipment")]
    public Item equippedWeapon;
    public Item equippedArmor;

    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel + 1];
        mpToAddNextLevel = new int[maxLevel];
        expToNextLevel[1] = startExp;
        for(int i = 2; i < expToNextLevel.Length-1; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i-1] * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            AddExp(100000);
        }
    }

    public void AddExp(int exp)
    {
        currentEXP += exp;
        Leveling();
        //GameManager.instance.UpdateInfo();
        //Debug.Log("1");
        if(OnStatsChanged != null)
        {
            OnStatsChanged(this);
        }
    }

    private void Leveling()
    {
        if (playerLevel == maxLevel)
        {
            currentEXP = 0;
        }

        if (playerLevel < maxLevel && currentEXP > expToNextLevel[playerLevel])
        {
            currentEXP -= expToNextLevel[playerLevel];

            // Improving our stats
            if(playerLevel % 2 == 0)
            {
                strength++;
            }else
            {
                defence++;
            }

            // For health
            maxHP = Mathf.FloorToInt(maxHP * 1.05f);
            currentHP = maxHP;
                
            // For MP
            maxMP += mpToAddNextLevel[playerLevel];
            currentMP = maxMP;
            playerLevel++;
  
            Leveling();
        }
    }
}
