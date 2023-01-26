using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("Load Last Saved Scene Or Game Menu")]
    public string mainMenu;
    public string loadScene;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(4);
        // MyPlayerController.instance.gameObject.SetActive(false);
        // GameMenu.instance.gameObject.SetActive(false);
        // BattleManager.instance.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitToMenu()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(MyPlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        SceneManager.LoadScene(mainMenu);
    }

    public void LoadLastSave()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(MyPlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);
        SceneManager.LoadScene(loadScene);
    }
}
