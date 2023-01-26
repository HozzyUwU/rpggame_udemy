using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    [Header("Elements: ")]
    public Text dialogueText;
    public Text nameText;
    public GameObject dialogueBox;
    public GameObject nameBox;
    public static DialogueScript instance;
    public string[] completeQuest;
    public string[] startQuest;

    [Header("Messages: ")]
    public string[] dialogueLines;
    public int currentLine;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //dialogueText.text = dialogueLines[currentLine];
    }

    // Update is called once per frame
    void Update()
    {
        // Checkin if dialogue box is active in this moment
        if(dialogueBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                currentLine++;
                if(currentLine < dialogueLines.Length)
                {
                    CheckForName();
                    dialogueText.text = dialogueLines[currentLine];
                }else
                {
                    dialogueBox.SetActive(false);
                    GameManager.instance.gameEvents["Dialogue"] = false;
                    // MyPlayerController.instance.canMove = true; // Now player can move

                    if(startQuest != null)
                    {
                        foreach(string quest in startQuest)
                        {
                            QuestManager.instance.MarkQuestIncomplete(quest);
                        }
                    }

                    if(completeQuest != null)
                    {
                        foreach(string quest in completeQuest)
                        {
                            QuestManager.instance.MarkQuestComplete(quest);
                        }
                    }
                }
            }
        }
    }

    // Function to communicate with this script
    public void SetDialogue(string[] newLines, bool isPerson, string[] _questsToComplete, string[] _questsToStart)
    {
        if(!dialogueBox.activeInHierarchy)
        {
            nameBox.SetActive(isPerson);
            dialogueLines = newLines;
            currentLine = 0;
            CheckForName();
            dialogueText.text = dialogueLines[currentLine];
            dialogueBox.SetActive(true);
            GameManager.instance.gameEvents["Dialogue"] = true;
            // MyPlayerController.instance.canMove = false; // Player no longer can move

            completeQuest = _questsToComplete;
            startQuest = _questsToStart; 
        }
    }
    
    // Function to change name in dialogue box
    public void CheckForName()
    {
        if(dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }
}
