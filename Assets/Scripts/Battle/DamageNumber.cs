using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [Header("Reference To Text GUI")]
    public TextMeshProUGUI damageText;

    [Header("Properties")]
    public float lifetime = 1f;
    public float moveSpeed = 1f;
    public float placementJitter = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
    }
}
