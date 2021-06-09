using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    private AudioSource audio;
    private bool isPlaying;

    void Start()
    {
        audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        isPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !isPlaying)
        {
            Debug.Log("Start music" + isPlaying);
            audio.Play();
            isPlaying = true;
        }
    }
}
