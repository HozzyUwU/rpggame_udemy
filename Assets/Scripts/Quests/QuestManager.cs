using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("References:")]
    public static QuestManager instance;
    public Dictionary<string, Quest> quests = new Dictionary<string, Quest>();
    public bool[] states;
    public Quest[] test;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // DontDestroyOnLoad(gameObject);
        foreach(Quest quest in test)
        {
            quest.questState = false;
            quest.isComplete = false;
            AddQuest(quest);
            // quest.StartQuest();
            // quest.questState = false;
            // quests.Add(quest.questName, quest);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.G))
        {
            foreach(KeyValuePair<string, Quest> obj in quests)
            {
                // Debug.Log(obj.Key + " " + obj.Value.isComplete);
                Debug.Log("QuestMarker_" + obj.Key + " " + PlayerPrefs.GetInt("QuestMarker_" + obj.Key) + " State: " +  PlayerPrefs.GetInt("QuestState_" + obj.Key));
                Debug.Log("QuestMarker_" + obj.Key + " " + obj.Value.isComplete + " State: " +  obj.Value.questState);
                
            }
        }

        // if(Input.GetKeyUp(KeyCode.O))
        // {
        //     SaveQuestData();
        // }
        // if(Input.GetKeyUp(KeyCode.P))
        // {
        //     LoadQuestData();
        // }
        // if(Input.GetKeyUp(KeyCode.U))
        // {
        //     ;
        // }     
    }

    public void MarkQuestComplete(string questName)
    {
        if(quests.ContainsKey(questName))
        {
            quests[questName].CompleteQuest();
        }
    }

    public void MarkQuestIncomplete(string questName)
    {
        if(quests.ContainsKey(questName))
        {
            quests[questName].StartQuest();
        }
    }

    public void AddQuest(Quest _quest)
    {
        if(quests.ContainsKey(_quest.questName))
        {
            quests[_quest.questName] = _quest;
        }else
        {
            quests.Add(_quest.questName, _quest);
        }
    }

    public void SaveQuestData()
    {
        foreach(KeyValuePair<string, Quest> obj in quests)
        {
            if(obj.Value.isComplete)
            {
                PlayerPrefs.SetInt("QuestMarker_" + obj.Key, 1);
            }else
            {
                PlayerPrefs.SetInt("QuestMarker_" + obj.Key, 0);
            }

            if(obj.Value.questState)
            {
                PlayerPrefs.SetInt("QuestState_" + obj.Key, 1);
            }else
            {
                PlayerPrefs.SetInt("QuestState_" + obj.Key, 0);
            }
        }
    }

    public void LoadQuestData()
    {
        // Figure out why there is no quests in dictinary at the start of scene
        Debug.Log(quests.Count);
        foreach(KeyValuePair<string, Quest> obj in quests)
        {
            obj.Value.isComplete = false;
            if(PlayerPrefs.HasKey("QuestMarker_" + obj.Key))
            {
                obj.Value.isComplete = PlayerPrefs.GetInt("QuestMarker_" + obj.Key) != 0;
                if(obj.Value.isComplete)
                {
                    obj.Value.CompleteQuest();
                }
            }

            obj.Value.questState = false;
            if(PlayerPrefs.HasKey("QuestState_" + obj.Key))
            {
                obj.Value.questState = PlayerPrefs.GetInt("QuestState_" + obj.Key) != 0;
                if(obj.Value.questState)
                {
                    obj.Value.StartQuest();
                }
                Debug.Log("Quests Data Was Loaded");
            }
        }
    }
}
