using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestsLog : MonoBehaviour
{
    public TextMeshProUGUI questDesc;
    public GameObject holder;
    public GameObject label;
    public Image image;

    private bool questsWasUpdatedOnLoad = false;

    // Start is called before the first frame update
    void Start()
    {
        // image.enabled = false;
        // label.SetActive(false);
        Quest.OnQuestsChanged -= UpdateQuestsInfo;
        Quest.OnQuestsChanged += UpdateQuestsInfo;
    }

    private void Update() 
    {
        if(!questsWasUpdatedOnLoad && QuestManager.instance != null  && SceneManager.GetActiveScene().buildIndex != 1)
        {
            questsWasUpdatedOnLoad = true;
            UpdateQuestsInfo();
        }

        if(Input.GetKeyUp(KeyCode.C))
        {
            UpdateQuestsInfo();
            // foreach(KeyValuePair<string, Quest> obj in QuestManager.instance.quests)
            // {
            //     Debug.Log(obj.Key + " " + obj.Value.questState);
                
            // }
        }

        if(!MyPlayerController.instance.canMove)
        {

        }
    }

    // Update is called once per frame
    public void UpdateQuestsInfo()
    {
        if(QuestManager.instance.quests.Count > 0) 
        {
            //gameObject.SetActive(false);
            if(holder != null)
            {
                foreach(Transform child in holder.transform)
                {
                    if(child.gameObject != label)
                    {
                        Destroy(child.gameObject);
                    }
                }
                holder.SetActive(false);
                foreach(KeyValuePair<string, Quest> quest in QuestManager.instance.quests)
                {
                    if(quest.Value.questState)
                    {
                        holder.SetActive(true);
                        questDesc.text = "-> " + quest.Value.questDescription;
                        Instantiate(questDesc, new Vector3 (0,0,0), Quaternion.identity, holder.transform);
                    }
                }
            }
        }
    }
}
