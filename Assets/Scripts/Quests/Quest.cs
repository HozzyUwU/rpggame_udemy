using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    // Creating delegate to notify other objects that quest was changed
    public delegate void QuestsChanged();
    public static event QuestsChanged OnQuestsChanged;

    [Header("Quest Properties")]
    public string questName;
    public bool questState;
    public bool isComplete;
    public string questDescription;
    [Header("Objects Connected To The Quest")]
    public QuestObject[] objects;

    // private bool questAdded = false;

    private void Start() {
        // foreach(QuestObject obj in objects)
        // {
        //     obj.connectedQuest = this;
        // }
    }

    private void Update() 
    {
        // if(!questAdded)
        // {
        //     questAdded = true;

        //     QuestManager.instance.AddQuest(this);
        // }
    }

    public void CompleteQuest()
    {
        questState = false;
        isComplete = true;
        // foreach(QuestObject obj in objects)
        // {
        //     obj.ProceedCompetedQuest();
        // }

        if(OnQuestsChanged != null)
        {
            OnQuestsChanged();
        }
    }

    public void StartQuest()
    {
        questState = true;
        isComplete = false;
        // foreach(QuestObject obj in objects)
        // {
        //     obj.ProceedActiveQuest();
        // }

        if(OnQuestsChanged != null)
        {
            OnQuestsChanged();
        }
    }
    
}
