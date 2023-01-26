using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("New Game")]
    public string newGameScene;

    [Header("Continue Game")]
    public string continueGame;

    [Header("References")]
    public GameObject contunieButton;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Current_Scene"))
        {
            contunieButton.SetActive(true);
        }else
        {
            contunieButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        SceneManager.LoadScene(continueGame);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
