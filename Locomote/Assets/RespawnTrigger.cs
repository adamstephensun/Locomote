using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public Transform respawnPoint;
    private AudioClip dieClip;
    private AudioClip successClip;
    public bool die;

    private void Start()
    {
        dieClip = Resources.Load<AudioClip>("Audio/Error.wav");
        successClip = Resources.Load<AudioClip>("Audio/Success.wav");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = respawnPoint.position;
            if(die) AudioSource.PlayClipAtPoint(dieClip, gameObject.transform.position);
            else AudioSource.PlayClipAtPoint(successClip, gameObject.transform.position);
        }
    }
}
