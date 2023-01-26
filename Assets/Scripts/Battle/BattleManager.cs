using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject eventSystem;
    public static BattleManager instance;

    public bool isFleeing;
    public bool isActive;

    [Header("Battle Scene")]
    public GameObject battleScene;

    [Space]
    [Header("Player Stats")]
    public TextMeshProUGUI[] playerName;
    public TextMeshProUGUI[] playerHP;
    public TextMeshProUGUI[] playerMP;

    [Space]
    [Header("Positions Of Player And Enemies")]
    public Transform[] allyPositions;
    public Transform[] enemyPositions;

    [Space]
    [Header("Prefabs For Player And Enemies")]
    public BattleChar[] allyPrefabs;
    public BattleChar[] enemyPrefabs;
    
    public List<BattleChar> activeBattlers = new List<BattleChar>();

    [Space]
    [Header("References")]
    public GameObject inventoryPanel;
    public GameObject buttonsHolder;
    public int currentTurn;
    public bool turnWaiting;
    public Notification battleNote;
    public GameObject battlersStats;
    public GameObject targetMenu;
    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;
    public BattleTargetButton[] targetButton;
    public string gameOverScene;

    [Space]
    [Header("Moves And Effects")]
    public BattleMove[] movesList;
    public GameObject enemyAttackingFX;
    public DamageNumber damageNumbers;

    [Space]
    [Header("Rewards")]
    public int rewardEXP;
    public string[] rewardItems;

    [Space]
    [Header("Properties")]
    public int chanceToFlee = 35;
    public bool cantFlee;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.X))
        {
            //BattleStart(new string[] {"Fligel", "Frogo", "Goblino"});
        }

        // Turn-based fight system
        if(isActive)
        {
            if(turnWaiting)
            {
                if(activeBattlers[currentTurn].isPlayer)
                {
                    buttonsHolder.SetActive(true);

                    // Player turn logic
                }else
                {
                    buttonsHolder.SetActive(false);

                    // Enemy turn logic
                    StartCoroutine(EnemyMoveCo());
                }
            }
        }
    }

    public void BattleStart(string[] enemiesToFight, bool _cantFlee)
    {
        if(!isActive)
        {
            cantFlee = _cantFlee;
            isActive = true;
            
            GameManager.instance.gameEvents["Battle Is Active"] = true;

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);
            battlersStats.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            // Loading characters
            for(int i = 0; i < allyPositions.Length; i++)
            {
                if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for(int j = 0; j < allyPrefabs.Length; j++)
                    {
                        // Debug.Log(GameManager.instance.playerStats[i].charName + " and " + allyPrefabs[j].charName);
                        if(allyPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            // Debug.Log(GameManager.instance.playerStats[i].charName);
                            BattleChar newPlayer = Instantiate(allyPrefabs[j], allyPositions[i].position, allyPositions[i].rotation);
                            newPlayer.transform.parent = allyPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats tempPlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = tempPlayer.currentHP;
                            activeBattlers[i].maxHP = tempPlayer.maxHP;
                            activeBattlers[i].currentMP = tempPlayer.currentMP;
                            activeBattlers[i].maxMP = tempPlayer.maxMP;
                            activeBattlers[i].strength = tempPlayer.strength;
                            activeBattlers[i].defence = tempPlayer.defence;
                            activeBattlers[i].wpnPower = tempPlayer.wpnPower;
                            activeBattlers[i].armrPower = tempPlayer.armrPower;
                        }
                    }
                }
            }

            // Loading enemies
            for(int i = 0; i < enemyPositions.Length; i++)
            {
                if(i < enemiesToFight.Length && enemiesToFight[i] != "")
                {
                    for(int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemyPrefabs[j].charName == enemiesToFight[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

            // Preps for start
            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);
            UpdateUIStats();

            // GameMenu.instance.eventSystem.SetActive(false);
            // eventSystem.SetActive(true);
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if(currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

        UpdateUIStats();
        UpdateBattle();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayerDead = true;

        foreach(BattleChar battler in activeBattlers)
        {
            if(battler.currentHP <= 0)
            {
                battler.currentHP = 0;

                // Handle death logic
                if(battler.isPlayer)
                {
                    battler.sprite.sprite = battler.deadSprite;
                }else
                {
                    battler.EnemyFade();
                }
            }else
            {
                if(battler.isPlayer)
                {
                    battler.sprite.sprite = battler.aliveSprite;
                    allPlayerDead = false;
                }else
                {

                    allEnemiesDead = false;
                }
            }
        }

        // Handling end of the battle
        if(allEnemiesDead || allPlayerDead)
        {
            if(allEnemiesDead)
            {
                // Produce win logic
                StartCoroutine(EndBattleCo());
            }else
            {
                // Produce lose logic
                StartCoroutine(GameOverCo());
            }
            // battleScene.SetActive(false);
            // activeBattlers.Clear();
            // GameManager.instance.gameEvents["Battle Is Active"] = false;
            // isActive = false;
        }else
        {
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        int selectedPlayer = players[Random.Range(0, players.Count)];

        // activeBattlers[selectedPlayer].currentHP -= 30;

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        foreach(BattleMove move in movesList)
        {
            if(move.moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(move.fx, activeBattlers[selectedPlayer].transform.position, activeBattlers[selectedPlayer].transform.rotation);
                DealDamage(selectedPlayer, move.movePower);
                Instantiate(enemyAttackingFX, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
            }
        }
    }

    public void DealDamage(int target, int power)
    {
        float attackPower = activeBattlers[currentTurn].wpnPower + activeBattlers[currentTurn].strength;
        float defencePower = activeBattlers[target].defence + activeBattlers[target].armrPower;

        float resultDamage = (attackPower / defencePower) * power * Random.Range(0.9f, 1.1f);
        Debug.Log(activeBattlers[currentTurn].charName + "is dealing " + resultDamage + " damage to " + activeBattlers[target].charName);
        
        activeBattlers[target].currentHP -= Mathf.FloorToInt(resultDamage);
        Instantiate(damageNumbers, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(-Mathf.FloorToInt(resultDamage));

        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for(int i = 0; i < playerName.Length; i++)
        {
            if(i < activeBattlers.Count)
            {
                if(activeBattlers[i].isPlayer)
                {
                    playerName[i].gameObject.SetActive(true);

                    playerName[i].text = activeBattlers[i].charName;
                    playerHP[i].text = Mathf.Clamp(activeBattlers[i].currentHP, 0, int.MaxValue) + " / " + activeBattlers[i].maxHP;
                    playerMP[i].text = Mathf.Clamp(activeBattlers[i].currentMP, 0, int.MaxValue) + " / " + activeBattlers[i].maxMP;
                }else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int target)
    {
        foreach(BattleMove move in movesList)
        {
            if(move.moveName == moveName)
            {
                Instantiate(move.fx, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
                Instantiate(enemyAttackingFX, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
                DealDamage(target, move.movePower);
            }
        }
        buttonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);
        List<int> enemies = new List<int>();
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(!activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                enemies.Add(i);
            }
        }

        for(int i = 0; i < targetButton.Length; i++)
        {
            if(i < enemies.Count)
            {
                targetButton[i].gameObject.SetActive(true);

                targetButton[i].moveName = moveName;
                targetButton[i].target = enemies[i];
                targetButton[i].targetName.text = activeBattlers[enemies[i]].charName;
            }else
            {
                targetButton[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        for(int i = 0; i < magicButtons.Length; i++)
        {
            if(i < activeBattlers[currentTurn].movesAvailable.Length)
            {
                magicButtons[i].gameObject.SetActive(true);
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].spellText.text = magicButtons[i].spellName;
                for(int j = 0; j < movesList.Length; j++)
                {
                    if(movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if(cantFlee)
        {
            // NextTurn();
            battleNote.text.text = "You Can't Escape From Boss!";
            battleNote.Activate();
        }else
        {
            int fleeSuccess = Random.Range(0, 100);
            if(fleeSuccess < chanceToFlee)
            {
                // Ending the battle
                // battleScene.SetActive(false);
                // activeBattlers.Clear();
                // GameManager.instance.gameEvents["Battle Is Active"] = false;
                // isActive = false;

                isFleeing = true;
                StartCoroutine(EndBattleCo());
            }else
            {
                NextTurn();
                battleNote.text.text = "You Can't Escape!";
                battleNote.Activate();
            }
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        inventoryPanel.GetComponent<CharacterInventory>().RefreshInventory();
    }

    public void UseItem(Item item, string charName)
    {
        int target = -1;
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].charName == charName)
            {
                target = i;
                i = activeBattlers.Count;
            }
        }

        if(item.affectHP)
        {
            activeBattlers[target].currentHP += item.affectingValue;
            activeBattlers[target].currentHP = Mathf.Clamp(activeBattlers[target].currentHP, 0, activeBattlers[target].maxHP);
        }
        else if(item.affectMP)
        {
            activeBattlers[target].currentMP += item.affectingValue;
            activeBattlers[target].currentMP = Mathf.Clamp(activeBattlers[target].currentMP, 0, activeBattlers[target].maxMP);
        }
        OpenInventory();
        NextTurn();
    }

    public IEnumerator EndBattleCo()
    {
        isActive = false;
        GameManager.instance.gameEvents["Battle Is Active"] = false;
        buttonsHolder.SetActive(false);
        battlersStats.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        UIFade.instance.fadeScreen.color = new Color(UIFade.instance.fadeScreen.color.r, UIFade.instance.fadeScreen.color.g, UIFade.instance.fadeScreen.color.b, 0);
        UIFade.instance.fadeScreen.gameObject.SetActive(true);
        // GameMenu.instance.eventSystem.SetActive(true);
        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);
        foreach(BattleChar battler in activeBattlers)
        {
            if(battler.isPlayer)
            {
                foreach(CharStats stat in GameManager.instance.playerStats)
                {
                    if(stat.charName == battler.charName)
                    {
                        stat.currentHP = battler.currentHP;
                        stat.currentMP = battler.currentMP;
                    }
                }
            }
            Destroy(battler);
        }
        battleScene.SetActive(false);
        UIFade.instance.FadeFromBlack();
        activeBattlers.Clear();
        currentTurn = 0;

        if(!isFleeing)
        {
            BattleReward.instance.OpenRewardScreen(rewardEXP , rewardItems);
        }else
        {
            isFleeing = false;
        }

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);

    }

    public IEnumerator GameOverCo()
    {
        // eventSystem.SetActive(false);
        // GameMenu.instance.eventSystem.SetActive(true);

        isActive = false;
        battlersStats.SetActive(false);
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(gameOverScene );

    }
}
