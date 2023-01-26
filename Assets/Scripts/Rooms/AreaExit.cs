using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [Header("Choose the scene to change to")]
    [Space]
    public string areaToLoadNext;
    [Header("Name of current area")]
    [Space]
    public string areaTransitionName;
    [Header("References")]
    [Space]
    public AreaEntrance theEntrance;
    [Header("Variables: ")]
    public float waitToLoad = 1f;

    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
    }

    // Changing the scene to another one
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            StartCoroutine(LoadNextscene());
        }
    }

    private IEnumerator LoadNextscene()
    {
        GameManager.instance.gameEvents["Fading"] = true;
        // MyPlayerController.instance.canMove = false;
        // Setting up dalay before changing the scene
        UIFade.instance.FadeToBlack();
        MyPlayerController.instance.areaTransitionName = areaTransitionName;
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(areaToLoadNext);
        UIFade.instance.FadeFromBlack();
        GameManager.instance.gameEvents["Fading"] = false;
        // MyPlayerController.instance.canMove = true;
    }
}
