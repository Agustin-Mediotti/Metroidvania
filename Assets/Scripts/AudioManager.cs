using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public GameObject soundObject;
    public AudioClip sfx_landing, sfx_jump;
    public AudioClip music_background;

    void Awake() { instance = this; }

    public void PlaySFX(string sfxName)
    {
        switch (sfxName)
        {
            case "landing":
                SoundObjectCreation(sfx_landing);
                break;
            case "jump":
                SoundObjectCreation(sfx_jump);
                break;
            default:
                break;
        }

    }

    void SoundObjectCreation(AudioClip clip)
    {
        GameObject newObject = Instantiate(soundObject, transform);
        newObject.GetComponent<AudioSource>().clip = clip;
        newObject.GetComponent<AudioSource>().Play();
    }
    public void PlayMusic(string musicName)
    {
        switch (musicName)
        {
            case "background":
                MusicObjectCreation(music_background);
                break;
            default:
                break;
        }

    }

    void MusicObjectCreation(AudioClip clip)
    {
        GameObject newObject = Instantiate(soundObject, transform);
        newObject.GetComponent<AudioSource>().clip = clip;
        newObject.GetComponent<AudioSource>().loop = true;
        newObject.GetComponent<AudioSource>().Play();
        //background music
        //AudioManager.instance.PlayMusic("StartMusic");
    }

}
