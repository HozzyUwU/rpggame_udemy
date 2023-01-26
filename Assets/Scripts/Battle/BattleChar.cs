using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    [Header("Player Or Enemy")]
    public bool isPlayer;

    [Space]
    [Header("Spells And Attacks")]
    public string[] movesAvailable;

    [Space]
    [Header("Character Stats")]
    public string charName;
    public int currentHP;
    public int maxHP;
    public int currentMP;
    public int maxMP;
    public int strength;
    public int defence;
    public int wpnPower;
    public int armrPower;

    [Space]
    [Header("Character State")]
    public bool hasDied;
    public SpriteRenderer sprite;
    public Sprite deadSprite;
    public Sprite aliveSprite;

    [Space]
    [Header("For Enemy To Die")]
    private bool shouldFade;
    public float fadeSpeed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldFade)
        {
            sprite.color = new Color(Mathf.MoveTowards(sprite.color.r, 1f, fadeSpeed * Time.deltaTime), 
            Mathf.MoveTowards(sprite.color.g, 0f, fadeSpeed * Time.deltaTime), 
            Mathf.MoveTowards(sprite.color.b, 0f, fadeSpeed * Time.deltaTime),
            Mathf.MoveTowards(sprite.color.a, 0f, fadeSpeed * Time.deltaTime));

            if(sprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void EnemyFade()
    {
        shouldFade = true;
    }
}
