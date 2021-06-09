using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public Transform respawnPoint;
    public AudioClip dieClip;
    public AudioClip successClip;
    public AudioClip lavaClip;
    public triggerType type;

    public enum triggerType
    {
        die,
        success,
        lava
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case triggerType.die:
                    AudioSource.PlayClipAtPoint(dieClip, gameObject.transform.position);
                    break;
                case triggerType.success:
                    AudioSource.PlayClipAtPoint(successClip, gameObject.transform.position);
                    break;
                case triggerType.lava:
                    AudioSource.PlayClipAtPoint(lavaClip, gameObject.transform.position);
                    break;
            }

            collision.gameObject.transform.position = respawnPoint.position;
        }
    }
}
