    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    [Header("Info to Display")]
    [Space]
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charHP;
    public TextMeshProUGUI charMP;
    public TextMeshProUGUI charLVL;
    public TextMeshProUGUI charTextExp;
    public Slider charBarExp;
    public Image charImage;
    
    [Header("Source")]
    [Space]
    public CharStats source;

    // Subscribing to an event
    private void Start() 
    {
        UpdateStats(source);
        CharStats.OnStatsChanged += UpdateStats;    
    }

    public void UpdateStats(CharStats _source)
    {
        if(source == _source)
        {
            this.charName.text = source.charName;
            this.charHP.text = "HP: " + source.currentHP + "/" + source.maxHP;
            this.charMP.text = "MP: " + source.currentMP + "/" + source.maxMP;
            this.charLVL.text = "LEVEL: " + source.playerLevel;
            this.charTextExp.text = source.currentEXP + "/" + source.expToNextLevel[source.playerLevel]; 
            //Debug.Log(((float)source.currentEXP / (float)source.expToNextLevel[source.playerLevel]));
            this.charBarExp.value = (float)source.currentEXP / (float)source.expToNextLevel[source.playerLevel];
            this.charImage.sprite = source.chrImage;
        }
    }
}
