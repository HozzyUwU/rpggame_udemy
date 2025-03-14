using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleMagicSelect : MonoBehaviour
{
    public string spellName;
    public int spellCost;
    public TextMeshProUGUI spellText;
    public TextMeshProUGUI costText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        if(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        }else
        {
            // Notify player that he has not enough mp
            BattleManager.instance.battleNote.text.text = "Not Enough MP!";
            BattleManager.instance.battleNote.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }
    }
}
