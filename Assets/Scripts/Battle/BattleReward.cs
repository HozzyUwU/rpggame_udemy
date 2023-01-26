using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleReward : MonoBehaviour
{
    public static BattleReward instance;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI itemText;
    public GameObject rewardScreen;
    public bool markQuestComplete;
    public string questToMark;
    public string[] rewardItems;
    public int expEarned;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRewardScreen(int xp, string[] items)
    {
        expEarned = xp;
        rewardItems = items;

        expText.text = "Everyone Earned " + expEarned + "xp!";
        itemText.text = "";

        foreach(string item in rewardItems)
        {
            itemText.text += item + "\n";
        }

        GameManager.instance.gameEvents["Reward Screen Is Active"] = true;
        rewardScreen.SetActive(true);
    }

    public void CloseRewardScreen()
    {
        foreach(CharStats stats in GameManager.instance.playerStats)
        {
            if(stats.gameObject.activeInHierarchy)
            {
                stats.AddExp(expEarned);
            }
        }

        foreach(string itemName in rewardItems)
        {
            foreach(Item item in GameManager.instance.availableItems)
            {
                if(item.name == itemName)
                {
                    GameManager.instance.AddItem(item);
                }
            }
        }
        
        rewardScreen.SetActive(false);
        GameManager.instance.gameEvents["Reward Screen Is Active"] = false;

        if(markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
