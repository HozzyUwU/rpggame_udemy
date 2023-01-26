using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;

    [Header("References: ")]
    public Image fadeScreen;

    [Header("Fade Variables: ")]
    public float fadeSpeed;

    // Booleans
    private bool shouldFadeToBlack;
    private bool shouldFadeFromBlack;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldFadeToBlack)
        {
            // Increasing alpha of picture
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 1f)
            {
                // fadeScreen.gameObject.SetActive(false);
                shouldFadeToBlack = false;
            }
        }
        else if(shouldFadeFromBlack)
        {
            // Decreasing alpha of picture
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                // fadeScreen.gameObject.SetActive(false);
                shouldFadeFromBlack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        fadeScreen.color = new Color(UIFade.instance.fadeScreen.color.r, UIFade.instance.fadeScreen.color.g, UIFade.instance.fadeScreen.color.b, 0);
        fadeScreen.gameObject.SetActive(true);
        // fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1);
        // fadeScreen.gameObject.SetActive(true);
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        fadeScreen.color = new Color(UIFade.instance.fadeScreen.color.r, UIFade.instance.fadeScreen.color.g, UIFade.instance.fadeScreen.color.b, 1);
        fadeScreen.gameObject.SetActive(true);
        // fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 0);
        // fadeScreen.gameObject.SetActive(true);
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}
