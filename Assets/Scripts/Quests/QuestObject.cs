using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    [Header("Active State After Quest")]
    public bool activeState;
    public bool completeState;
    public string connectedQuest;
    public GameObject GFX;
    // Funcction to affect to object after quest
    private void Update() 
    {
        if(QuestManager.instance.quests.ContainsKey(connectedQuest))
        {
            if(QuestManager.instance.quests[connectedQuest].questState)
            {
                ProceedActiveQuest();
            }
            if(QuestManager.instance.quests[connectedQuest].isComplete)
            {
                ProceedCompetedQuest();
            }
        }
    }

    public void ProceedCompetedQuest()
    {
        GFX.SetActive(completeState);
    }

    public void ProceedActiveQuest()
    {
        GFX.SetActive(activeState);
    }
}
