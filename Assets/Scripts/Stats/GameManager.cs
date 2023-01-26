using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void NeedToRefreshInventory();
    public static event NeedToRefreshInventory OnInventoryRefresh;

    public int test = 0;

    public static GameManager instance;
    public CharStats[] playerStats;
    [Header("Variavles to keep track of events in scene")]
    [Space]
    public CharacterPanel playerPanel;
    // Creating list to store new panels
    List<CharacterPanel> players = new List<CharacterPanel>();
    public Item[] availableItems;

    // Dictionary to store info about items
    public Dictionary<Item, int> playerItems = new Dictionary<Item, int>();

    public Dictionary<string, bool> gameEvents = new Dictionary<string, bool>()
    {
        {"Menu", false},
        {"Dialogue", false},
        {"Fading", false},
        {"Shop Menu", false},
        {"Battle Is Active", false},
        {"Reward Screen Is Active", false},
    };

    [SerializeField]
    private int currentGold;
    public int CurrentGold
    {
        get {return currentGold; }
        set 
        {
            currentGold = value;
            if(currentGold < 0)
            {
                currentGold = 0;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(bool state in gameEvents.Values)
        {
            if(state)
            {
                MyPlayerController.instance.canMove = false;
                return;
            }
            // Debug.Log("State " + state);
            MyPlayerController.instance.canMove = true;
        }

        if(Input.GetKeyUp(KeyCode.O))
        {
            QuestManager.instance.SaveQuestData();
            SaveData();
        }
        if(Input.GetKeyUp(KeyCode.P))
        {
            QuestManager.instance.LoadQuestData();
            LoadData();
        }
    }

    public Item getItemInfo(string itemName)
    {
        foreach(KeyValuePair<Item, int> item in playerItems)
        {
            if(item.Key.itemName == itemName)
            {
                return item.Key;
            }
        }

        return null;
    }

    public void RemoveItem(Item _item)
    {
        if(GameManager.instance.playerItems.ContainsKey(_item))
        {
            GameManager.instance.playerItems[_item]--;
            if(GameManager.instance.playerItems[_item] < 1)
            {
                GameManager.instance.playerItems.Remove(_item);
            }

            if(OnInventoryRefresh != null)
            {
                OnInventoryRefresh();
            }
        }
    }

    public void AddItem(Item _item)
    {
        if(!playerItems.ContainsKey(_item))
        {
            playerItems.Add(_item, 1);
        }else
        {
            playerItems[_item]++;
        }

        Debug.Log(_item.itemName + "was picked up, amount " + playerItems[_item]);

        if(OnInventoryRefresh != null)
        {
            OnInventoryRefresh();
        }
    }

    // public void UpdateInfo()
    // {
    //     if(players.Count > 0)
    //     {
    //         for(int i = 0; i < players.Count; i++)
    //         {
    //             players[i].UpdateStats(playerStats[i]);
    //         }
    //     }
    // }

    public void CreatePlayersPanels(GameObject holder)
    {
        players.Clear();
        foreach (Transform child in holder.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for(int i = 0; i < playerStats.Length; i++)
        {
            //layerPanel.UpdateStats(playerStats[i]);
            players.Add(Instantiate(playerPanel, new Vector3 (0,0,0), Quaternion.identity, holder.transform)); 
            players[i].source = playerStats[i];
        }
    }

    public void SaveData()
    {
        // Saving current scene
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);

        // Saving player position
        PlayerPrefs.SetFloat("Player_Position_X", MyPlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_Y", MyPlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_Z", MyPlayerController.instance.transform.position.z);

        // Saving stats for each characters
        foreach(CharStats stats in playerStats)
        {
            // Saving state
            if(stats.gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + stats.charName + "_isActive", 1);
            }else
            {
                PlayerPrefs.SetInt("Player_" + stats.charName + "_isActive", 0);
            }

            // Saving stats
            PlayerPrefs.SetInt("Player_" + stats.charName + "_Level", stats.playerLevel);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_CurrentEXP", stats.currentEXP);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_CurrentHP", stats.currentHP);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_MaxHP", stats.maxHP);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_CurrentMP", stats.currentMP);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_MaxMP", stats.maxMP);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_Strength", stats.strength);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_Defence", stats.defence);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_WpnPwr", stats.wpnPower);
            PlayerPrefs.SetInt("Player_" + stats.charName + "_ArmrPwr", stats.armrPower);
            // Saving items
            // PlayerPrefs.SetString("Player_" + stats.charName + "_EquippedWpn", stats.equippedWeapon.itemName);
            // PlayerPrefs.SetString("Player_" + stats.charName + "_EquippedArmr", stats.equippedArmor.itemName);

            // Store inventory
            foreach(Item item in availableItems)
            {
                if(playerItems.ContainsKey(item))
                {
                    PlayerPrefs.SetInt("Item_" + item.itemName, playerItems[item]);
                }else
                {
                    PlayerPrefs.SetInt("Item_" + item.itemName, 0);
                }
            }
        }
    }

    public void LoadData()
    {
        // Loading player position from last save
        MyPlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_X"), PlayerPrefs.GetFloat("Player_Position_Y"), PlayerPrefs.GetFloat("Player_Position_Z"));

        // Loading stats
        foreach(CharStats stats in playerStats)
        {
            if(PlayerPrefs.HasKey("Player_" + stats.charName + "_isActive"))
            {
                stats.gameObject.SetActive(PlayerPrefs.GetInt(("Player_" + stats.charName + "_isActive")) != 0);
            }

            // Loading stats
            stats.playerLevel = PlayerPrefs.GetInt("Player_" + stats.charName + "_Level");
            stats.currentEXP = PlayerPrefs.GetInt("Player_" + stats.charName + "_CurrentEXP");
            stats.currentHP = PlayerPrefs.GetInt("Player_" + stats.charName + "_CurrentHP");
            stats.maxHP = PlayerPrefs.GetInt("Player_" + stats.charName + "_MaxHP");
            stats.currentMP = PlayerPrefs.GetInt("Player_" + stats.charName + "_CurrentMP");
            stats.maxMP = PlayerPrefs.GetInt("Player_" + stats.charName + "_MaxMP");
            stats.strength = PlayerPrefs.GetInt("Player_" + stats.charName + "_Strength");
            stats.defence = PlayerPrefs.GetInt("Player_" + stats.charName + "_Defence");
            stats.wpnPower = PlayerPrefs.GetInt("Player_" + stats.charName + "_WpnPwr");
            stats.armrPower = PlayerPrefs.GetInt("Player_" + stats.charName + "_ArmrPwr");
            // Loading items
            // stats.equippedWeapon.itemName = PlayerPrefs.GetString("Player_" + stats.charName + "_EquippedWpn");
            // stats.equippedArmor.itemName = PlayerPrefs.GetString("Player_" + stats.charName + "_EquippedArmr");
        }
        // Loading inventory
            foreach(Item item in availableItems)
            {
                if(PlayerPrefs.HasKey("Item_" + item.itemName) && PlayerPrefs.GetInt("Item_" + item.itemName) > 0)
                {
                    playerItems.Add(item, PlayerPrefs.GetInt("Item_" + item.itemName));
                }
            }
    }
}
