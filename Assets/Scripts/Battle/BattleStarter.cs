using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleType[] potentialBattles;
    public bool activateOnEnter, activateOnStay, activateOnExit;
    private bool inArea;
    public float timeBetweenBattles;
    private float betweenBattleCounter;
    public bool deactivateAfterStarting;
    public bool cantFlee;
    public bool shouldCompleteQuest;
    public string questToComplete;

    // Start is called before the first frame update
    void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(inArea && MyPlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                betweenBattleCounter -= Time.deltaTime;
            }

            if(betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);

                StartCoroutine(StartBattleCo());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            if(activateOnEnter)
            {
                // Start the battle
                StartCoroutine(StartBattleCo());
            }else
            {
                inArea = true;
            }
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            if(activateOnExit)
            {
                // Start the battle
                StartCoroutine(StartBattleCo());
            }else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattleCo()
    {
        // UIFade.instance.fadeScreen.color = new Color(UIFade.instance.fadeScreen.color.r, UIFade.instance.fadeScreen.color.g, UIFade.instance.fadeScreen.color.b, 0);
        // UIFade.instance.fadeScreen.gameObject.SetActive(true);
        UIFade.instance.FadeToBlack();
        GameManager.instance.gameEvents["Battle Is Active"] = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardEXP = potentialBattles[selectedBattle].rewardExp;
        
        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cantFlee);
        UIFade.instance.FadeFromBlack();
        yield return new WaitForSeconds(1.5f);
        UIFade.instance.fadeScreen.gameObject.SetActive(false);

        if(deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }

        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = questToComplete;
    }
}
