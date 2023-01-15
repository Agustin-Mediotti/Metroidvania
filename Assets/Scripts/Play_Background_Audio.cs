using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Background_Audio : MonoBehaviour
{
    public AudioSource audioSource;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !audioSource.isPlaying)
        {
            //audioSource.GetComponent<AudioSource>().loop = true;
            audioSource.Play();
            
        }
    }
}
