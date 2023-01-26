using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharacterPanelStats : MonoBehaviour
{
    public GameObject textHolder;
    public GameObject buttonsHolder;
    public Image image;
    public Button button;
    public TextMeshProUGUI text;
    List<Button> buttonsList = new List<Button>();
    List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();


    // Start is called before the first frame update
    void Start()
    {
        CharStats.OnStatsChanged += UpdatePanel;
    }

    // Update is called once per frame
    void Update()
    {
        // Creating buttons for each character
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            if(buttonsHolder != null)
            {
                foreach (Transform child in buttonsHolder.transform) 
                {
                    GameObject.Destroy(child.gameObject);
                }
                if(GameManager.instance != null)
                {
                    buttonsList.Clear();
                    int counter = 0;
                    foreach(CharStats player in GameManager.instance.playerStats)
                    {
                        buttonsList.Add(Instantiate(button, new Vector3 (0,0,0), Quaternion.identity, buttonsHolder.transform));
                        buttonsList[counter].GetComponentInChildren<TextMeshProUGUI>().text = player.charName;
                        buttonsList[counter].GetComponent<Button>().onClick.AddListener(()=>{UpdatePanel(player);});
                        counter++;
                    }
                }else
                {
                    Debug.Log("null");
                }
            }
        }   
    }

    public void CreatePanel()
    {
        if(buttonsHolder != null)
            {
                foreach (Transform child in buttonsHolder.transform) 
                {
                    GameObject.Destroy(child.gameObject);
                }
                if(GameManager.instance != null)
                {
                    buttonsList.Clear();
                    int counter = 0;
                    foreach(CharStats player in GameManager.instance.playerStats)
                    {
                        buttonsList.Add(Instantiate(button, new Vector3 (0,0,0), Quaternion.identity, buttonsHolder.transform));
                        buttonsList[counter].GetComponentInChildren<TextMeshProUGUI>().text = player.charName;
                        buttonsList[counter].GetComponent<Button>().onClick.AddListener(()=>{UpdatePanel(player);});
                        counter++;
                    }
                }else
                {
                    Debug.Log("null");
                }
                buttonsList[0].GetComponent<Button>().onClick.Invoke();
            }
    }

    // Displaying info about character by clicking button assigned to it
    void UpdatePanel(CharStats _player)
    {
        if(textHolder != null)
        {
            // Destroying all previous elements of TextHolder to display new info
            foreach (Transform child in textHolder.transform) 
            {
                GameObject.Destroy(child.gameObject);
            }
            image.sprite = _player.chrImage;
            TextMeshProUGUI temp;
            Debug.Log(_player.charName);
            temp = Instantiate(text, new Vector3 (0,0,0), Quaternion.identity, textHolder.transform);
            temp.text = "Health Points: " + _player.currentHP;
            temp.text += "\n\nMagick Points: " + _player.currentMP;
            temp.text += "\n\nLevel: " + _player.playerLevel;
            temp.text += "\n\nStrength: " + _player.strength;
            temp.text += "\n\nDefence: " + _player.defence;
            if(_player.equippedWeapon != null)
            {
                temp.text += "\n\nEquipped Weapon: " + _player.equippedWeapon.itemName;
            }else
            {
                temp.text += "\n\nEquipped Weapon: " + null;
            }
            temp.text += "\n\nWeapon Power: " + _player.wpnPower;
            if(_player.equippedArmor != null)
            {
                temp.text += "\n\nEquipped Armor: " + _player.equippedArmor.itemName;
            }else
            {
                temp.text += "\n\nEquipped Armor: " + null;
            }
            temp.text += "\n\nArmor Power: " + _player.armrPower;
        }
    }
}
