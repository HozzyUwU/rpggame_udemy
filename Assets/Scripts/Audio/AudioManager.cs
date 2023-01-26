using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music And SFX")]
    public AudioSource[] sfx;
    public AudioSource[] bgm;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int sfxToPlay)
    {
        if(sfxToPlay < sfx.Length)
        {
            sfx[sfxToPlay].Play();
        }
    }

    public void PlayBGM(int bgmToPlay)
    {
        if(!bgm[bgmToPlay].isPlaying)
        {
            StopAllMusic();

            if(bgmToPlay < bgm.Length)
            {
                bgm[bgmToPlay].Play();
            }
        }
    }

    public void StopAllMusic()
    {
        foreach(AudioSource audio in bgm)
        {
            audio.Stop();
        }
    }   
}
