using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject eventSystem;

    public static GameMenu instance;
    [Header("Menu Objects")]
    public GameObject theMenu;
    public CharacterPanel playerPanel;
    public GameObject panelHolder;
    public GameObject goldImage;
    public TextMeshProUGUI goldAmount;
    // Store different panels to switch between
    public GameObject[] windows;
    [Header("Inventory Stuff")]
    public Item itemSelected;
    public TextMeshProUGUI itemSelectedName;
    public TextMeshProUGUI itemSelectedDescription;
    [Header("SFX For Menu")]
    public int sfxForOpening;
    [Header("Reference To Main Menu")]
    public string mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //UpdateInfoOnCharacterPanel();
        // GameManager.instance.CreatePlayersPanels(panelHolder);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotating UI gold icon
        goldImage.gameObject.transform.RotateAround(goldImage.gameObject.transform.position, Vector3.up, 100f * Time.deltaTime);
        if(GameManager.instance != null)
        {
            goldAmount.text = GameManager.instance.CurrentGold.ToString();
        }else
        {
            goldAmount.text = "0";
        }
        // goldImage.gameObject.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));

        
        // if(Input.GetKeyUp(KeyCode.O))
        // {
        //     GameManager.instance.CreatePlayersPanels(panelHolder);
        // }
        if(Input.GetKeyUp(KeyCode.I))
        {
            if(theMenu.activeInHierarchy)
            {
                CloseMenu();
                // MyPlayerController.instance.canMove = true;
            }else
            {
                CloseMenu();
                GameManager.instance.CreatePlayersPanels(panelHolder);
                // Debug.Log(GameManager.instance.gameEvents["Game Menu Open"]);
                // MyPlayerController.instance.canMove = false;
            }

            AudioManager.instance.PlaySFX(sfxForOpening);
        }
    }

    public void UpdateInfoOnCharacterPanel()
    {
        foreach (Transform child in panelHolder.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        // for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        // {
        //     //playerPanel.UpdateStats(GameManager.instance.playerStats[i]);
        //     Instantiate(playerPanel, new Vector3 (0,0,0), Quaternion.identity, panelHolder.transform); 
        // }

        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            //playerPanel.UpdateStats(GameManager.instance.playerStats[i]);
            Instantiate(playerPanel, new Vector3 (0,0,0), Quaternion.identity, panelHolder.transform); 
        }
    }

    // Function to switch between menu panels
    public void SwitchWindows(int windowID)
    {
        bool state = windows[windowID].activeInHierarchy;
        foreach(GameObject panel in windows)
        {
            panel.SetActive(false);
        }
        windows[windowID].SetActive(!state);
    }

    public void CloseMenu()
    {
        foreach(GameObject panel in windows)
        {
            panel.SetActive(false);
        }
        theMenu.SetActive(!theMenu.activeInHierarchy);
        GameManager.instance.gameEvents["Menu"] = theMenu.activeInHierarchy;
    }

    public void DisplaySelectedItemInfo(Item _item)
    {
        
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound(int sfxToPlay)
    {
        AudioManager.instance.PlaySFX(sfxToPlay);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenu);

        Destroy(GameManager.instance.gameObject);
        Destroy(MyPlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
